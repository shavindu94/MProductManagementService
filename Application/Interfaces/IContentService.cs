using Application.Common.AWS;
using Application.DtoObjects;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IContentService
    {
        Task<string> SaveContent(FileModel fileModel);
        Task<string> RemoveContent(string fileUrl);
    }
}
