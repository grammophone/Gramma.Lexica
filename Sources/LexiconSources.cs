using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gramma.GenericContentModel;
using Gramma.LanguageModel.TrainingSources;
using Gramma.Lexica.LexiconModel;

namespace Gramma.Lexica.Sources
{
	/// <summary>
	/// Ordered collection of <see cref="TrainingSource{Lemma}"/> elements,
	/// suitable for building a <see cref="Lexicon"/>.
	/// </summary>
	public class LexiconSources : ReadOnlySequence<TrainingSource<Lemma>>
	{
	}
}
