-- 1. جدول التصنيفات (CourseCategorys)
ALTER TABLE course.CourseCategorys ADD ReferenceId INT NULL; -- اجعله NOT NULL لو الداتا مجبرة
ALTER TABLE course.CourseCategorys 
ADD CONSTRAINT FK_CourseCategorys_Reference FOREIGN KEY (ReferenceId) REFERENCES dbo.Reference(Id);

-- 2. جدول الكورسات (Courses)
-- ملاحظة: إذا كان موجوداً بالفعل في الكورس تأكد فقط من وجود الـ Foreign Key
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('course.Courses') AND name = 'ReferenceId')
BEGIN
    ALTER TABLE course.Courses ADD ReferenceId INT NULL;
END
ALTER TABLE course.Courses 
ADD CONSTRAINT FK_Courses_Reference FOREIGN KEY (ReferenceId) REFERENCES dbo.Reference(Id);

-- 3. جدول المحاضرين (Instructors)
ALTER TABLE course.Instructors ADD ReferenceId INT NULL;
ALTER TABLE course.Instructors 
ADD CONSTRAINT FK_Instructors_Reference FOREIGN KEY (ReferenceId) REFERENCES dbo.Reference(Id);

-- 4. جدول الأوسام (CourseTags)
ALTER TABLE course.CourseTags ADD ReferenceId INT NULL;
ALTER TABLE course.CourseTags 
ADD CONSTRAINT FK_CourseTags_Reference FOREIGN KEY (ReferenceId) REFERENCES dbo.Reference(Id);