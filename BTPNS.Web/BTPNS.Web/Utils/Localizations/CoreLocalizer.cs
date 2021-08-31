using BTPNS.Contracts;
using Microsoft.Extensions.Localization;

namespace BTPNS.Web.Utils.Localizations
{
    public class CoreLocalizer<T> : ICoreLocalizer<T>
    {
        private readonly IStringLocalizer<T> stringLocalizer;

        public CoreLocalizer(IStringLocalizer<T> stringLocalizer)
        {
            this.stringLocalizer = stringLocalizer;
        }

        public string this[string name] => stringLocalizer[name].Value;
    }
}
