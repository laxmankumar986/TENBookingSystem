using CsvHelper.Configuration;
using CsvHelper;
using MediatR;
using System.Globalization;
using TENBookingSystem.Application.Commands.Uploads;
using TENBookingSystem.Application.Core;
using TENBookingSystem.Entities.Members;
using TENBookingSystem.Persistence;
using TENBookingSystem.Entities.Inventorys;
using TENBookingSystem.DTO.Uploads;
using TENBookingSystem.Application.Utility;

namespace TENBookingSystem.Application.Handlers.Uploads
{
    public class UploadInventoryCommandHandler : IRequestHandler<UploadInventoryCommand, Result<UplaodFileResult>>
    {
        private readonly DataContext _dataContext;
        public UploadInventoryCommandHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<Result<UplaodFileResult>> Handle(UploadInventoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                UplaodFileResult objResult = new UplaodFileResult();
                using var reader = new StreamReader(request.File.OpenReadStream());
                using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HeaderValidated = null, // Disables header validation
                    MissingFieldFound = null // Optional: Avoid errors for missing fields
                }))
                {
                    var records = csv.GetRecords<Inventory>().ToList();

                    string ErrorCheckMessage = ValidateUploadFiles(records); // inside ValidateUploadFiles method we can make
                                                                             // this more generic to make the input date as UTC and then check validaton for that
                                                                             // as of now it is validating date based on system date
                    if (string.IsNullOrEmpty(ErrorCheckMessage))
                    {
                        await _dataContext.Inventorys.AddRangeAsync(records, cancellationToken);
                        await _dataContext.SaveChangesAsync(cancellationToken);

                        objResult.StatusMessage = "Inventory data uploaded successfully";
                        return Result<UplaodFileResult>.Success(objResult);
                    }
                    else
                    {
                        objResult.ErrorMessage = ErrorCheckMessage;
                        return Result<UplaodFileResult>.Success(objResult);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
                // we can use Rest exception here, using custome middleware for custome exception handling
            }
        }
        static string ValidateUploadFiles(List<Inventory> members)
        {
            string ErrorMessage = "";
            for (int i = 0; i < members.Count; i++)
            {
                string expectedFormat = "yyyy-MM-dd"; // Change to the required format
                bool allMatch = HelperUtility.CheckDateFormat(members[i].ExpireDate.ToString(), expectedFormat);
                if (!allMatch)
                {
                    ErrorMessage = ErrorMessage + "Row Number " + (i + 1) + "Is having invalid date format " + members[i].ExpireDate.ToString();
                }
                //similarly we can add validation for all fiedls
            }
            return ErrorMessage;
        }
    }
}
