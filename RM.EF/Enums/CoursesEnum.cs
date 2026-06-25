using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.EF.Enums
{
    public enum CourseStatus
    {
        Draft = 1,
        PendingReview = 2,
        Approved = 3,
        Published = 4,
        Archived = 5
    }

    public enum CourseLevel
    {
        Beginner = 1,
        Intermediate = 2,
        Advanced = 3
    }

    public enum MaterialType
    {
        Pdf = 1,
        Word = 2,
        PowerPoint = 3,
        Zip = 4,
        Link = 5
    }
}
