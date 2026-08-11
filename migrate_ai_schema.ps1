Add-Type -AssemblyName "System.Data"

$connectionString = $env:CONNECTION_STRING
if (-not $connectionString) {
    $connectionString = $env:ConnectionStrings__DefaultConnection
}
if (-not $connectionString) {
    $connectionString = "Server=WELCOME\SQLEXPRESS;Database=AIPortfolioDb;Trusted_Connection=True;TrustServerCertificate=True;"
}

$conn = New-Object System.Data.SqlClient.SqlConnection($connectionString)

$sql = @"
-- TABLES
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'PromptTemplates' AND schema_id = SCHEMA_ID('Portfolio'))
BEGIN
    CREATE TABLE Portfolio.PromptTemplates (
        Id BIGINT IDENTITY(1,1) PRIMARY KEY,
        Feature VARCHAR(100) NOT NULL,
        Version INT NOT NULL,
        SystemPrompt NVARCHAR(MAX) NOT NULL,
        UserPromptTemplate NVARCHAR(MAX) NOT NULL,
        IsActive BIT NOT NULL DEFAULT 1,
        CreatedOn DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AIUsages' AND schema_id = SCHEMA_ID('Portfolio'))
BEGIN
    CREATE TABLE Portfolio.AIUsages (
        Id BIGINT IDENTITY(1,1) PRIMARY KEY,
        UserId BIGINT NOT NULL FOREIGN KEY REFERENCES [Identity].Users(Id),
        Feature VARCHAR(100) NOT NULL,
        Model VARCHAR(100) NOT NULL,
        PromptType VARCHAR(50) NOT NULL,
        InputTokens INT NOT NULL,
        OutputTokens INT NOT NULL,
        EstimatedCost DECIMAL(18, 6) NOT NULL,
        DurationMs INT NOT NULL,
        Status VARCHAR(20) NOT NULL,
        CreatedOn DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AIChatSessions' AND schema_id = SCHEMA_ID('Portfolio'))
BEGIN
    CREATE TABLE Portfolio.AIChatSessions (
        Id BIGINT IDENTITY(1,1) PRIMARY KEY,
        UserId BIGINT NOT NULL FOREIGN KEY REFERENCES [Identity].Users(Id),
        Title NVARCHAR(250) NOT NULL,
        CreatedOn DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
        UpdatedOn DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AIChatMessages' AND schema_id = SCHEMA_ID('Portfolio'))
BEGIN
    CREATE TABLE Portfolio.AIChatMessages (
        Id BIGINT IDENTITY(1,1) PRIMARY KEY,
        SessionId BIGINT NOT NULL FOREIGN KEY REFERENCES Portfolio.AIChatSessions(Id) ON DELETE CASCADE,
        [Role] VARCHAR(20) NOT NULL,
        Content NVARCHAR(MAX) NOT NULL,
        InputTokens INT NOT NULL,
        OutputTokens INT NOT NULL,
        CreatedOn DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
    );
END
GO

-- PROCEDURES
CREATE OR ALTER PROCEDURE Portfolio.usp_PromptTemplate_GetByFeature
    @Feature VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TOP 1 Id, Feature, Version, SystemPrompt, UserPromptTemplate, IsActive, CreatedOn
    FROM Portfolio.PromptTemplates
    WHERE Feature = @Feature AND IsActive = 1
    ORDER BY Version DESC;
END;
GO

CREATE OR ALTER PROCEDURE Portfolio.usp_AIUsage_Create
    @UserId BIGINT,
    @Feature VARCHAR(100),
    @Model VARCHAR(100),
    @PromptType VARCHAR(50),
    @InputTokens INT,
    @OutputTokens INT,
    @EstimatedCost DECIMAL(18, 6),
    @DurationMs INT,
    @Status VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Portfolio.AIUsages (UserId, Feature, Model, PromptType, InputTokens, OutputTokens, EstimatedCost, DurationMs, Status, CreatedOn)
    VALUES (@UserId, @Feature, @Model, @PromptType, @InputTokens, @OutputTokens, @EstimatedCost, @DurationMs, @Status, SYSUTCDATETIME());
    SELECT SCOPE_IDENTITY();
END;
GO

CREATE OR ALTER PROCEDURE Portfolio.usp_AIChatSession_List
    @UserId BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, UserId, Title, CreatedOn, UpdatedOn
    FROM Portfolio.AIChatSessions
    WHERE UserId = @UserId
    ORDER BY UpdatedOn DESC;
END;
GO

CREATE OR ALTER PROCEDURE Portfolio.usp_AIChatSession_Create
    @UserId BIGINT,
    @Title NVARCHAR(250)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Portfolio.AIChatSessions (UserId, Title, CreatedOn, UpdatedOn)
    VALUES (@UserId, @Title, SYSUTCDATETIME(), SYSUTCDATETIME());
    SELECT SCOPE_IDENTITY();
END;
GO

CREATE OR ALTER PROCEDURE Portfolio.usp_AIChatMessage_List
    @SessionId BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, SessionId, [Role], Content, InputTokens, OutputTokens, CreatedOn
    FROM Portfolio.AIChatMessages
    WHERE SessionId = @SessionId
    ORDER BY CreatedOn ASC;
END;
GO

CREATE OR ALTER PROCEDURE Portfolio.usp_AIChatMessage_Create
    @SessionId BIGINT,
    @Role VARCHAR(20),
    @Content NVARCHAR(MAX),
    @InputTokens INT,
    @OutputTokens INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Portfolio.AIChatMessages (SessionId, [Role], Content, InputTokens, OutputTokens, CreatedOn)
    VALUES (@SessionId, @Role, @Content, @InputTokens, @OutputTokens, SYSUTCDATETIME());
    
    UPDATE Portfolio.AIChatSessions
    SET UpdatedOn = SYSUTCDATETIME()
    WHERE Id = @SessionId;
    
    SELECT SCOPE_IDENTITY();
END;
GO
"@

try {
    $conn.Open()
    $commands = $sql -split "(?m)^\s*GO\s*$"
    foreach ($cmdText in $commands) {
        if ([string]::IsNullOrWhiteSpace($cmdText)) { continue }
        $cmd = $conn.CreateCommand()
        $cmd.CommandText = $cmdText
        [void]$cmd.ExecuteNonQuery()
    }
    Write-Output "AI tables and stored procedures migrated successfully."
} catch {
    Write-Error $_
} finally {
    if ($conn.State -eq [System.Data.ConnectionState]::Open) {
        $conn.Close()
    }
}
