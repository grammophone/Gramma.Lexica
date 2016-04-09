using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grammophone.GenericContentModel;
using Grammophone.LanguageModel.Provision;

namespace Grammophone.Lexica.Sources
{
	/// <summary>
	/// Collection of <see cref="LexiconSourceSet"/> elements,
	/// keyed by <see cref="LanguageProvider"/>.
	/// </summary>
	public class LexiconSourceSets : ReadOnlyMultiMap<LanguageProvider, LexiconSourceSet>
	{
	}
}
