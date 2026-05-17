using BLL.DTOs;
using DAL.EF.Table;

namespace BLL
{
    public static class MapperConfig
    {
        // Doctor Mapping
        public static DoctorDTO ToDoctorDTO(Doctor doctor)
        {
            return new DoctorDTO
            {
                DoctorId = doctor.DoctorId,
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                Speciality = doctor.Speciality,
                ConsultationFee = doctor.ConsultationFee,
                Email = doctor.Email,
                IsAvailable = doctor.IsAvailable
            };
        }

        public static Doctor ToDoctorEntity(DoctorDTO doctorDto)
        {
            return new Doctor
            {
                DoctorId = doctorDto.DoctorId,
                FirstName = doctorDto.FirstName,
                LastName = doctorDto.LastName,
                Speciality = doctorDto.Speciality,
                ConsultationFee = doctorDto.ConsultationFee,
                Email = doctorDto.Email,
                IsAvailable = doctorDto.IsAvailable
            };
        }

        // Patient Mapping
        public static PatientDTO ToPatientDTO(Patient patient)
        {
            return new PatientDTO
            {
                PatientId = patient.PatientId,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Email = patient.Email,
                Phone = patient.Phone,
                Dob = patient.Dob,
                CreatedAt = patient.CreatedAt
            };
        }

        public static Patient ToPatientEntity(PatientDTO patientDto)
        {
            return new Patient
            {
                PatientId = patientDto.PatientId,
                FirstName = patientDto.FirstName,
                LastName = patientDto.LastName,
                Email = patientDto.Email,
                Phone = patientDto.Phone,
                Dob = patientDto.Dob,
                CreatedAt = patientDto.CreatedAt
            };
        }
    }
}