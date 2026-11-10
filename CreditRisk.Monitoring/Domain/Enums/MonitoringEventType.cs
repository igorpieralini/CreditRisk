namespace CreditRisk.Monitoring.Domain.Enums;

public enum MonitoringEventType
{
    CreditAnalyzed = 1,
    GuaranteeUpdated = 2,
    ExposureIncreased = 3,
    ExposureReduced = 4,
    CreditApproved = 5,
    CreditRejected = 6,
    ManualReviewRequired = 7,
    RiskAlertGenerated = 8
}