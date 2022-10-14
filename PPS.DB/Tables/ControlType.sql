CREATE TABLE [dbo].[ControlType] (
    [Id]          INT          IDENTITY (1, 1) NOT NULL,
    [Type]        VARCHAR (50) NOT NULL,
    [CreatedBy]   INT          NOT NULL,
    [CreatedDate] DATETIME     NOT NULL,
    [UpdatedBy]   INT          NOT NULL,
    [UpdatedDate] DATETIME     NOT NULL,
    CONSTRAINT [PK_dbo.ControlType] PRIMARY KEY CLUSTERED ([Id] ASC)
);

