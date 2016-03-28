# Gramma.Lexica
This .NET library defines the structure of lexica and sets the contract for importers of lexica. 

A lexicon is associated with a `LanguageProvider` defined in [Gramma.LanguageModel](https://github.com/grammophone/Gramma.LanguageModel) library and allows searching for the description of a lemma with a given maximum generalized edit distance. The generalized edit distance metrics are defined by the lexicon's `LanguageProvider`.
The search results are a collection of `LemmaResult` objects, which contain the distance from the queried lemma as well as the lemma found which in turn holds its senses and etymology. The lexica are available via the `LexicaEnvironment.Lexica` static property. The above are shown in the following UML diagram.

![Lexica domain](http://s24.postimg.org/shphwn5wl/Lexica_domain.png)

Importers of lexica from various sources must derive from the `TrainingSource<Lemma>` abstract class and must be registered in the `LexiconSources` property of `LexiconSourceSet` instances which in turn are members of the `LexiconSourceSets` collection of the `LexicaSetup` singleton. This singleton is available through the static property `LexicaEnvironment.Setup`, which is loaded from XAML representation, as shown in the [Gramma.Lexica.Importer](https://github.com/grammophone/Gramma.Lexica.Importer) project. This framework is shown in the UML diagram below.

![Lexica importing](http://s13.postimg.org/meqb1mq9j/Lexica_importing.png)

This projects relies on the following projects being in sibling directories:
* [Gramma.Configuration](https://github.com/grammophone/Gramma.Configuration)
* [Gramma.DataStreaming](https://github.com/grammophone/Gramma.DataStreaming)
* [Gramma.GenericContentModel](https://github.com/grammophone/Gramma.GenericContentMoel)
* [Gramma.Indexing](https://github.com/grammophone/Gramma.Indexing)
* [Gramma.LanguageModel](https://github.com/grammophone/Gramma.LanguageModel)
* [Gramma.Serialization](https://github.com/grammophone/Gramma.Serialization)

