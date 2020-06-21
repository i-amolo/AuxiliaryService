using AuxiliaryService.API.Shared.StateMachine.DTO;
using AuxiliaryService.Domain.StateMachine;
using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.ApplicationServices.Shared.StateMachine
{

    public static class StateMachineConverter
    {
        public static StateMachineDto Convert<TContext>(StateMachineData<TContext> source) where TContext: StateMachineBaseContext
        {
            if (source == null)
                return null;

            var dto = new StateMachineDto();
            dto.Code = source.Code;
            dto.Description = source.Description;
            dto.Error = source.Error;
            dto.InstanceId = source.InstanceId;
            dto.RefId = source.RefId;
            dto.StateMachineId = source.StateMachineId;
            dto.Status = source.Status;
            dto.Progress = JsonConvert.SerializeObject(source.Progress);
            dto.Context = JsonConvert.SerializeObject(source.Context);
            return dto;
        }

        public static StateMachineData<TContext> Convert<TContext>(StateMachineDto source) where TContext : StateMachineBaseContext
        {
            if (source == null)
                return null;

            var data = new StateMachineData<TContext>();
            data.Code = source.Code;
            data.Description = source.Description;
            data.Error = source.Error;
            data.InstanceId = source.InstanceId;
            data.RefId = source.RefId;
            data.StateMachineId = source.StateMachineId;
            data.Status = source.Status;
            data.Progress = JsonConvert.DeserializeObject<List<StateMachineTransitionRunInfo>>(source.Progress);
            data.Context = JsonConvert.DeserializeObject<TContext>(source.Context);
            return data;
        }

    }

}
