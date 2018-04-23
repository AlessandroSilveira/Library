using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Library.Helpers
{
	public class ArrayModelBinder : IModelBinder
	{
		public Task BindModelAsync(ModelBindingContext bindingContext)
		{
			if (!bindingContext.ModelMetadata.IsEnumerableType)
			{
				bindingContext.Result = ModelBindingResult.Failed();
				return Task.CompletedTask;
			}

			var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).ToString();

			if (string.IsNullOrWhiteSpace(value))
			{
				bindingContext.Result = ModelBindingResult.Success(null);
				return Task.CompletedTask;
			}

			var elementType = bindingContext.ModelType.GetType().GenericTypeArguments[0];
			var converter = TypeDescriptor.GetConverter(elementType);
		}
	}
}