using Courses.BLL.DTO;
using System.Collections.Generic;

namespace Courses.BLL.Interfaces
{
    public interface ISpecializationService
    {
        void Create(SpecializationDto item);
        SpecializationDto GetSpecialization(int? id);
        List<SpecializationDto> GetSpecializations();
        void DeleteSpecialization(int? id);
        void UpdateSpecialization(SpecializationDto specialization);
        void Dispose();
    }
}
