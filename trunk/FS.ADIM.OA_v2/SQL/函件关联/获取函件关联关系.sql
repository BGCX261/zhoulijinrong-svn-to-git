USE DB_Adim
GO
IF OBJECT_ID (N'dbo.GetRelationIDRight', N'FN') IS NOT NULL
    DROP FUNCTION dbo.GetRelationIDRight
GO
CREATE FUNCTION dbo.GetRelationIDRight (@ProcessType varchar(50),@ProcessID varchar(50),@ID int)
returns varchar(max)
AS
BEGIN
DECLARE @Num1 varchar(50)
DECLARE @ProID varchar(50)
	--向后找
	IF (Charindex('函件发文',@ProcessType)>0)
	BEGIN
		--获取函件发文的发问号、对方发问号
		SET @Num1=(
		SELECT TOP 1 DocumentNo
		FROM dbo.T_OA_HF_WorkItems_BAK		
		WHERE ProcessID=@ProcessID and DocumentNo<>'' order by ID desc)
		--根据发文号找对应答复文号的收文ID
		IF ISNULL(@Num1,'')<>''
		BEGIN
			SELECT TOP 1 @ProID=ProcessID 
			FROM dbo.T_OA_HS_Edit 
			where ReplyDocumentNo=@Num1
			if(@ProID<>'')
			BEGIN
				if(@ID<>0)
					return '##'+@ProcessType+'#'+@ProcessID+'#'+cast(@ID as varchar(10))+dbo.GetRelationIDRight('函件收文',@ProID,@ID+1)
				else
					return dbo.GetRelationIDRight('函件收文',@ProID,@ID+1)
			END
			ELSE
			BEGIN
				if(@ID<>0)
					return '##'+@ProcessType+'#'+@ProcessID+'#'+cast(@ID as varchar(10))
			END
		END
	END
	ELSE IF(Charindex('函件收文',@ProcessType)>0)
	BEGIN
		--获取函件收文的文件编码、答复文号
		SET @Num1=(SELECT TOP 1 FileEncoding
		FROM dbo.T_OA_HS_Edit 
		WHERE ProcessID=@ProcessID)
		IF ISNULL(@Num1,'')<>''
		BEGIN
			SELECT TOP 1 @ProID=ProcessID 
			FROM dbo.T_OA_HF_WorkItems_BAK 
			WHERE CAST(FormsData.query('EntityLetterSend/yourRef/text()' )as varchar(50))=@Num1	
			if(@ProID<>'')
			BEGIN
				if(@ID<>0)
					return '##'+@ProcessType+'#'+@ProcessID+'#'+cast(@ID as varchar(10))+dbo.GetRelationIDRight('函件发文',@ProID,@ID+1)
				else
					return dbo.GetRelationIDRight('函件发文',@ProID,@ID+1)
			END
			ELSE
			BEGIN
				if(@ID<>0)
					return '##'+@ProcessType+'#'+@ProcessID+'#'+cast(@ID as varchar(10))
			END
		END	
	END
	if(@ID<>0)
		return '##'+@ProcessType+'#'+@ProcessID+'#'+cast(@ID as varchar(10))
	return ''	
END
GO
IF OBJECT_ID (N'dbo.GetRelationIDLeft', N'FN') IS NOT NULL
    DROP FUNCTION dbo.GetRelationIDLeft
GO
CREATE FUNCTION dbo.GetRelationIDLeft (@ProcessType varchar(50),@ProcessID varchar(50),@ID int)
returns varchar(max)
AS
BEGIN
DECLARE @Num1 varchar(50)
DECLARE @ProID varchar(50)
	--向前找
	IF (Charindex('函件发文',@ProcessType)>0)
	BEGIN
		--获取函件发文的对方发文号
		SET @Num1=(
		SELECT TOP 1 CAST(FormsData.query('EntityLetterSend/yourRef/text()')AS varchar(50)) as yourRef  
		FROM dbo.T_OA_HF_WorkItems_BAK		
		WHERE ProcessID=@ProcessID and CAST(FormsData.query('EntityLetterSend/yourRef/text()')AS varchar(50))<>'' order by ID desc)
		--根据对方发文号找对文件编码号的收文ID
		IF ISNULL(@Num1,'')<>''
		BEGIN
			SELECT TOP 1 @ProID=ProcessID 
			FROM dbo.T_OA_HS_Edit 
			where FileEncoding=@Num1
			if(@ProID<>'')
			BEGIN
				if(@ID<>0)
					return dbo.GetRelationIDLeft('函件收文',@ProID,@ID-1)+'##'+@ProcessType+'#'+@ProcessID+'#'+cast(@ID as varchar(10))
				else
					return dbo.GetRelationIDLeft('函件收文',@ProID,@ID-1)
				END
			ELSE
			BEGIN
				if(@ID<>0)
					return '##'+@ProcessType+'#'+@ProcessID+'#'+cast(@ID as varchar(10))
			END
		END
	END
	ELSE IF(Charindex('函件收文',@ProcessType)>0)
	BEGIN
		--获取函件收文的答复文号
		SET @Num1=(SELECT TOP 1 ReplyDocumentNo
		FROM dbo.T_OA_HS_Edit 
		WHERE ProcessID=@ProcessID)
		--根据答复文号获取对应发文号的发文ID
		IF ISNULL(@Num1,'')<>''
		BEGIN
			SELECT TOP 1 @ProID=ProcessID 
			FROM dbo.T_OA_HF_WorkItems_BAK 
			WHERE DocumentNo=@Num1
			if(@ProID<>'')
			BEGIN
				if(@ID<>0)
					return dbo.GetRelationIDLeft('函件发文',@ProID,@ID-1)+'##'+@ProcessType+'#'+@ProcessID+'#'+cast(@ID as varchar(10))
				else
					return dbo.GetRelationIDLeft('函件发文',@ProID,@ID-1)
			END
			ELSE
			BEGIN
				if(@ID<>0)
					return '##'+@ProcessType+'#'+@ProcessID+'#'+cast(@ID as varchar(10))
			END
		END	
	END
	if(@ID<>0)
		return '##'+@ProcessType+'#'+@ProcessID+'#'+cast(@ID as varchar(10))
	return ''	
END
GO
IF OBJECT_ID (N'dbo.GetRelationIDs', N'FN') IS NOT NULL
    DROP FUNCTION dbo.GetRelationIDs
GO
CREATE FUNCTION dbo.GetRelationIDs (@ProcessType varchar(50),@ProcessID varchar(50))
returns varchar(max)
AS
BEGIN
	return dbo.GetRelationIDLeft(@ProcessType,@ProcessID,0)+'##'+@ProcessType+'#'+@ProcessID+'#0'+dbo.GetRelationIDRight(@ProcessType,@ProcessID,0)
END