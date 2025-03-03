using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TENBookingSystem.Application.Core;
using TENBookingSystem.DTO.Uploads;

namespace TENBookingSystem.Application.Commands.Uploads
{
    public class UploadMemberCommand: IRequest<Result<UplaodFileResult>>
    {
        public IFormFile File { get; set; }

        public UploadMemberCommand(IFormFile file)
        {
            File = file;
        }
    }    
}
