
---->>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
---->>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>



--获取系统用户操作列表
--EXEC P_GetAdminOperationList 260
CREATE PROC [dbo].[P_GetAdminOperationList] @LoginId INT
AS
    BEGIN
        SELECT  so.OperationId ,
                so.OperationName ,
                LOWER(so.OperationUrl) AS OperationUrl
        FROM    dbo.SysOperation (NOLOCK) so
                LEFT JOIN dbo.SysModule smd ON so.ModuleId = smd.ModuleId
                INNER JOIN ( SELECT srp.ObjectId
                             FROM   dbo.SysRolePermission (NOLOCK) srp --角色操作
                                    INNER JOIN dbo.SysUserRoleMapping (NOLOCK) srm ON srm.RoleId = srp.RoleId
                                                              AND srm.LoginId = @LoginId
                             WHERE  srp.ObjectType = 1 --[Display(Name = "对象类型（0菜单 1 操作）")]
                             UNION
                             SELECT sup.ObjectId
                             FROM   SysUserPermission (NOLOCK) sup --临时权限
                             WHERE  sup.ObjectType = 1
                                    AND sup.LoginId = @LoginId
                                    AND sup.IsActive = 1
                                    AND CONVERT(VARCHAR(10), GETDATE(), 120) BETWEEN sup.StartDate
                                                              AND
                                                              sup.EndDate
                           ) sm ON sm.ObjectId = so.OperationId
        WHERE   so.IsActive = 1
        ORDER BY so.Sort DESC;
    END;

EXEC P_GetAdminOperationList 3881;


---->>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
---->>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>



----------------------------------
--生成日期基础表
----------------------------------
-- 设置每周的起始天为周一
SET DATEFIRST 1;
IF OBJECT_ID('Base_DateTimeInfo', 'U') IS NOT NULL
    DROP TABLE Base_DateTimeInfo;
GO

CREATE TABLE Base_DateTimeInfo
    (
      DateKey INT PRIMARY KEY ,
      DateTime DATE NOT NULL ,
      [DateName] NVARCHAR(20) ,
      DayNumberOfWeek TINYINT NOT NULL ,
      DayNameOfWeek NVARCHAR(10) NOT NULL ,
      DayNumberOfMonth TINYINT NOT NULL ,
      DayNumberOfYear SMALLINT NOT NULL ,
      WeekNumberOfYear TINYINT NOT NULL ,
      EnglishMonthName NVARCHAR(10) NOT NULL ,
      MonthNumberOfYear TINYINT NOT NULL ,
      CalendarQuarter TINYINT NOT NULL ,
      CalendarSemester TINYINT NOT NULL ,
      CalendarYear SMALLINT NOT NULL
    );

DECLARE @StartDate DATETIME;
DECLARE @EndDate DATETIME;

SELECT  @StartDate = '2015-01-01' ,
        @EndDate = '2035-12-31';

WHILE ( @StartDate <= @EndDate )
    BEGIN
        INSERT  INTO Base_DateTimeInfo
                ( DateKey ,
                  DateTime ,
                  [DateName] ,
                  DayNumberOfWeek ,
                  DayNameOfWeek ,
                  DayNumberOfMonth ,
                  DayNumberOfYear ,
                  WeekNumberOfYear ,
                  EnglishMonthName ,
                  MonthNumberOfYear ,
                  CalendarQuarter ,
                  CalendarSemester ,
                  CalendarYear 
                )
                SELECT  CAST(CONVERT(VARCHAR(8), @StartDate, 112) AS INT) AS DateKey ,
                        CONVERT(VARCHAR(10), @StartDate, 20) AS DateTime ,
                        CONVERT(VARCHAR(20), @StartDate, 106) AS [DateName] ,
                        DATEPART(DW, @StartDate) AS DayNumberOfWeek ,
                        DATENAME(DW, @StartDate) AS DayNameOfWeek ,
                        DATENAME(DD, @StartDate) AS [DayOfMonth] ,
                        DATENAME(DY, @StartDate) AS [DayOfYear] ,
                        DATEPART(WW, @StartDate) AS WeekNumberOfYear ,
                        DATENAME(MM, @StartDate) AS EnglishMonthName ,
                        DATEPART(MM, @StartDate) AS MonthNumberOfYear ,
                        DATEPART(QQ, @StartDate) AS CalendarQuarter ,
                        CASE WHEN DATEPART(MM, @StartDate) BETWEEN 1 AND 6
                             THEN 1
                             ELSE 2
                        END AS CalendarSemester ,
                        DATEPART(YY, @StartDate) AS CalendarYear; 
            
        SET @StartDate = @StartDate + 1;
    END;
GO