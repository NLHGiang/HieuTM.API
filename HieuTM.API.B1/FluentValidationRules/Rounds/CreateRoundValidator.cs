//namespace HieuTM.API.B1.FluentValidationRules.Round
//{
//    public partial class CreateRoundValidator : AbstractValidator<RoundCreateRequest>
//    {
//        private readonly AppReadOnlyDbContext _dbContext = new();

//        public CreateRoundValidator()
//        {
//            RuleFor(x => x.Name)
//                .Cascade(CascadeMode.Stop)
//                .NotEmpty().WithMessage(LocalizationString.Common.EmptyField);

//            RuleFor(x => x.StartTime)
//                .Cascade(CascadeMode.Stop)
//                .NotEmpty().WithMessage(LocalizationString.Common.EmptyField);

//            RuleFor(x => x.EndTime)
//                .Cascade(CascadeMode.Stop)
//                .NotEmpty().WithMessage(LocalizationString.Common.EmptyField);

//            RuleFor(x => x.TotalTime)
//                .Cascade(CascadeMode.Stop)
//                .NotEmpty().WithMessage(LocalizationString.Common.EmptyField);

//            RuleFor(x => x.Description)
//                .Cascade(CascadeMode.Stop)
//                .NotEmpty().WithMessage(LocalizationString.Common.EmptyField);

//            RuleFor(x => x.TakeTop)
//                .GreaterThanOrEqualTo(0)
//                    .WithMessage("Bảng xếp hạng phải là số không âm.");

//            RuleFor(x => x.MaxAccess)
//                .GreaterThanOrEqualTo(0)
//                    .WithMessage("Giới hạn truy cập phải là số không âm.");

//            RuleFor(x => x.StartTime)
//                .MustAsync(DateStartToDateContest)
//                .WithMessage("Thời gian bắt đầu phải nằm trong khoảng của vòng thi")
//                .When(x => x.StartTime.HasValue);

//            RuleFor(x => x.EndTime)
//                .MustAsync(DateStartToDateContest)
//                .WithMessage("Thời gian kết thúc phải nằm trong khoảng của vòng thi")
//                .When(x => x.EndTime.HasValue);

//            RuleFor(x => x.StartTime)
//                .LessThanOrEqualTo(x => x.EndTime)
//                .WithMessage("Thời gian bắt đầu phải bé hơn thời gian kết thúc")
//                .When(x => x.StartTime.HasValue);

//            RuleFor(x => x.EndTime)
//                .GreaterThanOrEqualTo(x => x.StartTime)
//                .WithMessage("Thời gian kết thúc phải lớn hơn thời gian bắt đầu")
//                .When(x => x.EndTime.HasValue);

//            RuleFor(x => x.IsFinal)
//                .MustAsync(IsFinals)
//                .WithMessage("Vòng trung kết đã có không thể thêm vòng trung kết")
//                .When(x => x.IsFinal);

//            RuleFor(x => x.EndTime)
//                .MustAsync(IsFinalsDate)
//                .WithMessage("Cuộc thi này đã có vòng trung kết vui lòng để ý thời gian thêm")
//                .When(x => x.EndTime.HasValue);

//            RuleFor(x => x.EndTime)
//                .MustAsync(ExistEnd)
//                .WithMessage("Ngày kết thúc này đã tồn tại")
//                .When(x => x.EndTime.HasValue);

//            RuleFor(x => x.StartTime)
//                .MustAsync(ExistStart)
//                .WithMessage("Ngày bắt đầu này đã tồn tại")
//                .When(x => x.StartTime.HasValue);

//            RuleFor(x => x.StartTime)
//                .MustAsync(ExistStartAfter)
//                .WithMessage("Ngày bắt đầu trùng với ngày kết thúc của vòng trước")
//                .When(x => x.StartTime.HasValue);

//            RuleFor(x => x.EndTime)
//                .MustAsync(ExistStartAfter)
//                .WithMessage("Ngày kết thúc trùng với ngày bắt đầu của vòng trước")
//                .When(x => x.EndTime.HasValue);

//        }

//        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propetyName) =>
//        {
//            var result = await ValidateAsync(ValidationContext<RoundCreateRequest>.CreateWithOptions((RoundCreateRequest)model, x => x.IncludeProperties(propetyName)));
//            if (result.IsValid) return Array.Empty<string>();
//            return result.Errors.Select(e => e.ErrorMessage);
//        };

//        public async Task<bool> DateStartToDateContest(RoundCreateRequest model, DateTimeOffset? startTime, CancellationToken cancellationToken)
//        {
//            var idContest = await _dbContext.ContestEntities.AsNoTracking().FirstOrDefaultAsync(c => c.Status == Domain.Enums.EntityStatus.Active && c.Id == model.IdContest && (startTime < c.StartTime || startTime > c.EndTime), cancellationToken);
//            if (idContest == null)
//            {
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }
//        public async Task<bool> DateEndToDateContest(RoundCreateRequest model, DateTimeOffset? endTime, CancellationToken cancellationToken)
//        {
//            var idContest = await _dbContext.ContestEntities.AsNoTracking().FirstOrDefaultAsync(c => c.Status == Domain.Enums.EntityStatus.Active && c.Id == model.IdContest && (endTime < c.StartTime || endTime > c.EndTime), cancellationToken);
//            if (idContest == null)
//            {
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }
//        public async Task<bool> IsFinals(RoundCreateRequest model, bool isFinal, CancellationToken cancellationToken)
//        {
//            var isfinal = await _dbContext.RoundEntities.AsNoTracking().AnyAsync(c => c.Status == Domain.Enums.EntityStatus.Active && c.IdContest == model.IdContest && c.IsFinal);
//            if (!isfinal)
//            {
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }
//        public async Task<bool> IsFinalsDate(RoundCreateRequest model, DateTimeOffset? endTime, CancellationToken cancellationToken)
//        {
//            var startTimeFn = await _dbContext.RoundEntities
//                .AsNoTracking()
//                .Where(r => r.IdContest == model.IdContest && r.Status == Domain.Enums.EntityStatus.Active && r.IsFinal)
//                .OrderByDescending(r => r.StartTime)
//                .Select(r => r.StartTime)
//                .FirstOrDefaultAsync(cancellationToken);
//            if (startTimeFn < endTime)
//            {
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }
//        public async Task<bool> ExistStart(RoundCreateRequest model, DateTimeOffset? startTime, CancellationToken cancellationToken)
//        {
//            var startTimeFn = await _dbContext.RoundEntities
//                .AsNoTracking()
//                .Where(r => r.IdContest == model.IdContest && r.Status == Domain.Enums.EntityStatus.Active && r.StartTime == startTime)
//                .FirstOrDefaultAsync(cancellationToken);
//            if (startTimeFn == null)
//            {
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }
//        public async Task<bool> ExistEnd(RoundCreateRequest model, DateTimeOffset? endTime, CancellationToken cancellationToken)
//        {
//            var endTimeFn = await _dbContext.RoundEntities
//                .AsNoTracking()
//                .Where(r => r.IdContest == model.IdContest && r.Status == Domain.Enums.EntityStatus.Active && r.EndTime == endTime)
//                .FirstOrDefaultAsync(cancellationToken);
//            if (endTimeFn == null)
//            {
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }
//        public async Task<bool> ExistEndAfter(RoundCreateRequest model, DateTimeOffset? endTime, CancellationToken cancellationToken)
//        {
//            var endTimeFn = await _dbContext.RoundEntities
//                .AsNoTracking()
//                .Where(r => r.IdContest == model.IdContest && r.Status == Domain.Enums.EntityStatus.Active && r.StartTime == endTime)
//                .FirstOrDefaultAsync(cancellationToken);
//            if (endTimeFn == null)
//            {
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }
//        public async Task<bool> ExistStartAfter(RoundCreateRequest model, DateTimeOffset? startTime, CancellationToken cancellationToken)
//        {
//            var endTimeFn = await _dbContext.RoundEntities
//                .AsNoTracking()
//                .Where(r => r.IdContest == model.IdContest && r.Status == Domain.Enums.EntityStatus.Active && r.EndTime == startTime)
//                .FirstOrDefaultAsync(cancellationToken);
//            if (endTimeFn == null)
//            {
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }
//    }
//}
