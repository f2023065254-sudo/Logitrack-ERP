CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    UserName NVARCHAR(100) NOT NULL UNIQUE,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    Password NVARCHAR(256) NOT NULL,
    Role NVARCHAR(50) NOT NULL
);


CREATE TABLE Attendance (
    AttendanceID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    EmployeeName NVARCHAR(100) NOT NULL,
    Role NVARCHAR(50) NOT NULL,
    AttendanceDate DATE NOT NULL,
    Status NVARCHAR(20) NOT NULL -- Present, Absent, Leave
);


WITH CTE AS (
    SELECT 
        AttendanceID, 
        UserID, 
        AttendanceDate,
        ROW_NUMBER() OVER(
            PARTITION BY UserID, AttendanceDate 
            ORDER BY AttendanceID DESC
        ) as RowNum
    FROM Attendance
)
-- Yeh query har date aur user ke purane duplicates delete kar degi aur sirf 1 latest record rakhegi
DELETE FROM CTE WHERE RowNum > 1;