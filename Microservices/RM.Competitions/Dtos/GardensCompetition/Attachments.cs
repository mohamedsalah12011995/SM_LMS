using RM.Core.Helpers;
using System;
using System.Collections.Generic;

namespace RM.Competitions.Dtos
{
    public class Attachments
    {
        public List<AttachmentBase> ProjectDrawingsImages { get; set; }
        public List<AttachmentBase> ProjectDrawingsAutocad { get; set; }
        public List<AttachmentBase> ProjectDescription { get; set; }
        public List<AttachmentBase> ProjectPresentation { get; set; }
        public List<AttachmentBase> ProjectVideos { get; set; }
        public class AttachmentBase
        {
            public int? _id { get; set; }
            public int? _typeId { get; set; }
            public int? _competitorId { get; set; }
            public string ID { set { _id = Accessor.Set(value); } get { return Accessor.Get(_id); } }
            public string TypeId { set { _typeId = Accessor.Set(value); } get { return Accessor.Get(_typeId); } }
            public string CompetitorId { set { _competitorId = Accessor.Set(value); } get { return Accessor.Get(_competitorId); } }
            public string FileName { get; set; }
            public string FileBase64 { get; set; }
            public DateTime? CreatedDate { get; set; }
        }

    }
}
