Although the **SquidEyes.TechAnalysis** library is more than adequate for the author's own  purposes, it should be fairly easy to add additional indicators to the library, if desired.  Just note that a few simple rules will need to be followed when submitting a pull-request:

* Indicators must be named according to the **{Code or Name}Indicator** convention, with well-known codes such as "Rsi" preferred over more verbose names like "Relative Strength Index."
* Indicators SHOULD NOT include an in-built result-storage method
  * As a rule, the storage of results should be entirely up to the consumer
  * Various collection types may be employed to store indicator results,  depending upon your requirements, but for many scenarios, the included **SlidingBuffer** should be your most convenient and performant alternative
  * Internally, indicators should strive to persist minimal data; most conveniently  through fixed-size forward or reversed SlidingBuffers
* Indicators may ONLY be driven by candlestick values, via an  "**AddAndCalc**" method (required) and an "**UpdateAndCalc**" method (optional)
  * The AddAndCalc and UpdateAndCalc methods should return an immutable "result" class, based on ResultBase; with Kind and OpenOn values, as well as one or more custom data fields
* Be sure to include a copy of the standard license header in each new source code file you create
  * This can be done most conveniently by using the Visual Studio LicenseHeader extension
  * Once installed, simply invoke the "License Headers" / "Add License Headers To All Files" command on the project containing new files
* No changes to the solution-wide .editorconfig file will be accepted
* Pull-requests will not be accepted without a full-coverage unit-test for each new or updated indicator
