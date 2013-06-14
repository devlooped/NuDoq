v0.4
* Added support for `returns` tag.
* Added support for retrieving `IXmlLineInfo` for every element, which allows much better diagnostics
* ToString now renders basic element information 
* Full element text content rendering moved to `ToText` method

v0.3
* Support for unknown XML elements (preserves and processes content)
* Support for writing XML from the in-memory model via XmlVisitor

v0.2

* Support for all documentation tags
* Support for custom visitors over the entire results
* Support for anonymous (delegate-based visitor) for convenience
* Additional member post-processing annotations from reflection if available (i.e. reading from an assembly)

v0.1

* Initial cut with visitable documentation elements