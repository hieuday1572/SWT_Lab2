using System;
using System.Collections.Generic;

namespace LMMProject.flm;

public partial class Session
{
    public int SessionId { get; set; }

    public string? Topic { get; set; }

    public string? LearningTeachingType { get; set; }

    public string? StudentMaterials { get; set; }

    public string? ConstructiveQuestion { get; set; }

    public string SubjectCode { get; set; } = null!;
}
