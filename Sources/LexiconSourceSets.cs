using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gramma.GenericContentModel;
using Gramma.LanguageModel.Provision;

namespace Gramma.Lexica.Sources
{
	/// <summary>
	/// Collection of <see cref="LexiconSourceSet"/> elements,
	/// keyed by <see cref="LanguageProvider"/>.
	/// </summary>
	public class LexiconSourceSets : ReadOnlyMultiMap<LanguageProvider, LexiconSourceSet>
	{
	}
}
