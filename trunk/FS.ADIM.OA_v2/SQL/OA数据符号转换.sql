update T_OA_GF_WorkItems 
set 
DocumentTitle=replace(replace(replace(DocumentTitle,'&quot;','"'),'&lt;','<'),'&gt;','>'),
FormsData=replace(replace(replace(cast(FormsData as varchar(max)),'&amp;quot;','"'),'&amp;lt;','&lt;'),'&amp;gt;','&gt;')

update T_OA_HF_WorkItems 
set 
FormsData=replace(replace(cast(FormsData as varchar(max)),'&lt;br/&gt;',char(13)+char(10)),'&amp;nbsp',' ')

update T_OA_WR_WorkItems 
set 
[Content]=replace(replace(cast([Content] as varchar(max)),'<br/>',char(13)+char(10)),'&nbsp',' '),
FormsData=replace(replace(cast(FormsData as varchar(max)),'&lt;br/&gt;',char(13)+char(10)),'&amp;nbsp',' ')

update T_OA_RR_WorkItems 
set 
[Content]=replace(replace(cast([Content] as varchar(max)),'<br/>',char(13)+char(10)),'&nbsp',' '),
FormsData=replace(replace(cast(FormsData as varchar(max)),'&lt;br/&gt;',char(13)+char(10)),'&amp;nbsp',' ')

update T_OA_HS_WorkItems 
set 
DocumentTitle=replace(DocumentTitle,'&quot;','"')



update T_OA_GF_WorkItems_BAK 
set 
DocumentTitle=replace(replace(replace(DocumentTitle,'&quot;','"'),'&lt;','<'),'&gt;','>'),
FormsData=replace(replace(replace(cast(FormsData as varchar(max)),'&amp;quot;','"'),'&amp;lt;','&lt;'),'&amp;gt;','&gt;')

update T_OA_HF_WorkItems_BAK  
set 
FormsData=replace(replace(cast(FormsData as varchar(max)),'&lt;br/&gt;',char(13)+char(10)),'&amp;nbsp',' ')

update T_OA_WR_WorkItems_BAK  
set 
[Content]=replace(replace(cast([Content] as varchar(max)),'<br/>',char(13)+char(10)),'&nbsp',' '),
FormsData=replace(replace(cast(FormsData as varchar(max)),'&lt;br/&gt;',char(13)+char(10)),'&amp;nbsp',' ')

update T_OA_RR_WorkItems_BAK  
set 
[Content]=replace(replace(cast([Content] as varchar(max)),'<br/>',char(13)+char(10)),'&nbsp',' '),
FormsData=replace(replace(cast(FormsData as varchar(max)),'&lt;br/&gt;',char(13)+char(10)),'&amp;nbsp',' ')

update T_OA_HS_WorkItems_BAK  
set 
DocumentTitle=replace(DocumentTitle,'&quot;','"')



