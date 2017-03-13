CREATE TABLE [dbo].[SpatialData] (
    [Timestamp] DATETIMEOFFSET (0) NOT NULL,
    [Latitude]  DECIMAL (9, 6)     NOT NULL,
    [Longitude] DECIMAL (9, 6)     NOT NULL,
    [Point]     AS                 ([geography]::Point([Latitude],[Longitude],(4326))) PERSISTED,
    PRIMARY KEY CLUSTERED ([Timestamp] ASC)
);

