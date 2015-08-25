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
	--�����
	IF (Charindex('��������',@ProcessType)>0)
	BEGIN
		--��ȡ�������ĵķ��ʺš��Է����ʺ�
		SET @Num1=(
		SELECT TOP 1 DocumentNo
		FROM dbo.T_OA_HF_WorkItems_BAK		
		WHERE ProcessID=@ProcessID and DocumentNo<>'' order by ID desc)
		--���ݷ��ĺ��Ҷ�Ӧ���ĺŵ�����ID
		IF ISNULL(@Num1,'')<>''
		BEGIN
			SELECT TOP 1 @ProID=ProcessID 
			FROM dbo.T_OA_HS_Edit 
			where ReplyDocumentNo=@Num1
			if(@ProID<>'')
			BEGIN
				if(@ID<>0)
					return '##'+@ProcessType+'#'+@ProcessID+'#'+cast(@ID as varchar(10))+dbo.GetRelationIDRight('��������',@ProID,@ID+1)
				else
					return dbo.GetRelationIDRight('��������',@ProID,@ID+1)
			END
			ELSE
			BEGIN
				if(@ID<>0)
					return '##'+@ProcessType+'#'+@ProcessID+'#'+cast(@ID as varchar(10))
			END
		END
	END
	ELSE IF(Charindex('��������',@ProcessType)>0)
	BEGIN
		--��ȡ�������ĵ��ļ����롢���ĺ�
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
					return '##'+@ProcessType+'#'+@ProcessID+'#'+cast(@ID as varchar(10))+dbo.GetRelationIDRight('��������',@ProID,@ID+1)
				else
					return dbo.GetRelationIDRight('��������',@ProID,@ID+1)
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
	--��ǰ��
	IF (Charindex('��������',@ProcessType)>0)
	BEGIN
		--��ȡ�������ĵĶԷ����ĺ�
		SET @Num1=(
		SELECT TOP 1 CAST(FormsData.query('EntityLetterSend/yourRef/text()')AS varchar(50)) as yourRef  
		FROM dbo.T_OA_HF_WorkItems_BAK		
		WHERE ProcessID=@ProcessID and CAST(FormsData.query('EntityLetterSend/yourRef/text()')AS varchar(50))<>'' order by ID desc)
		--���ݶԷ����ĺ��Ҷ��ļ�����ŵ�����ID
		IF ISNULL(@Num1,'')<>''
		BEGIN
			SELECT TOP 1 @ProID=ProcessID 
			FROM dbo.T_OA_HS_Edit 
			where FileEncoding=@Num1
			if(@ProID<>'')
			BEGIN
				if(@ID<>0)
					return dbo.GetRelationIDLeft('��������',@ProID,@ID-1)+'##'+@ProcessType+'#'+@ProcessID+'#'+cast(@ID as varchar(10))
				else
					return dbo.GetRelationIDLeft('��������',@ProID,@ID-1)
				END
			ELSE
			BEGIN
				if(@ID<>0)
					return '##'+@ProcessType+'#'+@ProcessID+'#'+cast(@ID as varchar(10))
			END
		END
	END
	ELSE IF(Charindex('��������',@ProcessType)>0)
	BEGIN
		--��ȡ�������ĵĴ��ĺ�
		SET @Num1=(SELECT TOP 1 ReplyDocumentNo
		FROM dbo.T_OA_HS_Edit 
		WHERE ProcessID=@ProcessID)
		--���ݴ��ĺŻ�ȡ��Ӧ���ĺŵķ���ID
		IF ISNULL(@Num1,'')<>''
		BEGIN
			SELECT TOP 1 @ProID=ProcessID 
			FROM dbo.T_OA_HF_WorkItems_BAK 
			WHERE DocumentNo=@Num1
			if(@ProID<>'')
			BEGIN
				if(@ID<>0)
					return dbo.GetRelationIDLeft('��������',@ProID,@ID-1)+'##'+@ProcessType+'#'+@ProcessID+'#'+cast(@ID as varchar(10))
				else
					return dbo.GetRelationIDLeft('��������',@ProID,@ID-1)
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