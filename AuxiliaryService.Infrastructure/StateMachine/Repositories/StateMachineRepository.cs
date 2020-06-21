using AuxiliaryService.Domain.StateMachine;
using AuxiliaryService.Domain.StateMachine.Queries;
using AuxiliaryService.Domain.StateMachine.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.Infrastructure.Repositories.StateMachine
{
    public class StateMachineRepository : IStateMachineRepository
    {

        private readonly DatabaseFactory _databaseFactory;
        private readonly IStateMachineQueries _queries;

        public StateMachineRepository(DatabaseFactory databaseFactory,
                                      IStateMachineQueries queries)
        {
            _databaseFactory = databaseFactory;
            _queries = queries;
        }

        public StateMachineDto Get(Guid instanceId)
        {
            using (var db = _databaseFactory.CreateDatabase())
            {
                return db.SingleOrDefault<StateMachineDto>(_queries.Select_GetStateMachineByInstanceId(), new { instanceId });
            }
        }

        public StateMachineDto Get(string code, Guid refId)
        {
            using (var db = _databaseFactory.CreateDatabase())
            {
                return db.SingleOrDefault<StateMachineDto>(_queries.Select_GetStateMachineByCodeRef(), new { code, refId });
            }
        }

        public void Save(StateMachineDto dto)
        {
            if (dto.StateMachineId == Guid.Empty)
            {
                Insert(dto);
            }
            else
            {
                Update(dto);
            }
        }

        private void Insert(StateMachineDto dto)
        {
            using (var db = _databaseFactory.CreateDatabase())
            {
                dto.StateMachineId = Guid.NewGuid();
                db.Insert(dto);
            }
        }

        private void Update(StateMachineDto dto)
        {
            using (var db = _databaseFactory.CreateDatabase())
            {
                db.Update(dto);
            }
        }

    }
}
