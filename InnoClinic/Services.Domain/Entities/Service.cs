﻿public class Service : Entity
{
    public int ServiceCategoryId { get; set; }
    public string ServiceName { get; set; }
    public Decimal? ServicePrice { get; set; }
    public int SpecializationId { get; set; }
    public bool IsActive { get; set; }
}
