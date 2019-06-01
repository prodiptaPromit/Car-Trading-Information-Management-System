select Model.BodyType from Model cross join SalesRecord where SalesRecord.ModelId=Model.Id

select COUNT(Id) from SalesRecord