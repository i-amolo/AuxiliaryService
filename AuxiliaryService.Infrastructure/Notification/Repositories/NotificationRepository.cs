using AuxiliaryService.Domain.Notification;
using AuxiliaryService.Domain.Notification.Queries;
using AuxiliaryService.Domain.Notification.Repositories;
using Newtonsoft.Json;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.Infrastructure.Notification.Repositories
{
    public class NotificationRepository : INotificationRepository
    {

        #region ctor

        public NotificationRepository(Framework.Core.Data.Orm.DatabaseFactory databaseFactory, INotificationQueries queries)
        {
            _databaseFactory = databaseFactory;
            _queries = queries;
        }

        #endregion 

        #region private fields

        private readonly Framework.Core.Data.Orm.DatabaseFactory _databaseFactory;
        private readonly INotificationQueries _queries;

        #endregion

        #region private methods

        private NotificationDto ToDto<TProviderDetails>(NotificationDomain<TProviderDetails> obj) where TProviderDetails : NotificationProviderDetailsDomainBase
        {
            var dto = new NotificationDto()
            {
                Id = obj.Id.GetValueOrDefault(), 
                RefId = obj.RefId, 
                ProviderType = obj.ProviderType,
                Status = obj.Status,
                Message = JsonConvert.SerializeObject(obj.Message),
                ProviderDetails = JsonConvert.SerializeObject(obj.ProviderDetails)
            };

            return dto;
        }

        private NotificationDomain<TProviderDetails> FromDto<TProviderDetails>(NotificationDto dto) where TProviderDetails : NotificationProviderDetailsDomainBase
        {
            var obj = new NotificationDomain<TProviderDetails>();
            obj.Id = dto.Id;
            obj.RefId = dto.RefId;
            obj.ProviderType = dto.ProviderType;
            obj.Status = dto.Status;
            obj.Message = JsonConvert.DeserializeObject<NotificationMessageDomain>(dto.Message);
            obj.ProviderDetails = JsonConvert.DeserializeObject<TProviderDetails>(dto.ProviderDetails);
            return obj;
        }

        private void Insert(NotificationDto dto)
        {
            using (var db = _databaseFactory.CreateDatabase())
            {
                dto.Id = Guid.NewGuid();
                dto.SysCreatedOn = DateTime.Now;
                dto.SysCreatedById = ApplicationContext.Worker.Id;
                db.Insert(dto);
            }
        }

        private void Update(NotificationDto dto)
        {
            using (var db = _databaseFactory.CreateDatabase())
            {
                db.Update(dto);
            }
        }


        #endregion

        #region INotificationRepository

        public NotificationDomain<TProviderDetails> Save<TProviderDetails>(NotificationDomain<TProviderDetails> notification) where TProviderDetails : NotificationProviderDetailsDomainBase
        {

            var dto = ToDto(notification);

            if (dto.Id == Guid.Empty)
            {
                Insert(dto);
                notification.Id = dto.Id;
            }
            else
            {
                Update(dto);
            }

            return notification;
        }

        public IEnumerable<NotificationDomain<TProviderDetails>> Get<TProviderDetails>(List<Guid> ids, List<string> refIds, string providerType) where TProviderDetails : NotificationProviderDetailsDomainBase
        {
            var sqlBuilder = new SqlBuilder();

            var obj = new
            {
                ids = ids != null && ids.Any() ? String.Join(",", ids) : null,
                refIds = refIds != null && refIds.Any() ? String.Join(",", refIds) : null,
                providerType
            };

            var tmp = sqlBuilder.AddTemplate(_queries.Select_Notifications(), obj);

            if (obj.ids != null)
            {
                sqlBuilder.Where("notification_id in (@ids)", obj);
            }

            if (obj.refIds != null)
            {
                sqlBuilder.Where("ref_id in (@refIds)", obj);
            }

            using (var db = _databaseFactory.CreateDatabase())
            {
                var dtos = db.Fetch<NotificationDto>(tmp);
                return dtos.Select(a => FromDto<TProviderDetails>(a));
            }
        }

        public void SetNotificationSent<TProviderDetails>(NotificationDomain<TProviderDetails> notification) where TProviderDetails : NotificationProviderDetailsDomainBase
        {
            using (var db = _databaseFactory.CreateDatabase())
            {
                db.Execute(_queries.Update_Sent(), new
                    {
                        Id = notification.Id.ToString(),
                        notification.Status,
                        notification.SentOn, 
                        SentDetails = (string)null
                    } 
                );
            }
        }

        public void SetNotificationError<TProviderDetails>(NotificationDomain<TProviderDetails> notification, Exception e) where TProviderDetails : NotificationProviderDetailsDomainBase
        {
            using (var db = _databaseFactory.CreateDatabase())
            {
                db.Execute(_queries.Update_Sent(), new
                    {
                        Id = notification.Id.ToString(),
                        notification.Status,
                        notification.SentOn,
                        SentDetails = JsonConvert.SerializeObject(new { Error = e.ToString()} )
                    }
                );
            }
        }

        #endregion


    }
}
