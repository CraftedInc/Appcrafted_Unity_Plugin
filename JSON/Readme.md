# Boomlagoon JSON

Boomlagoon JSON is a lightweight C# JSON implementation.

Boomlagoon JSON doesn't throw exceptions, and there is no casting 
involved, instead all valid JSON values are accessible as the correct 
C# types.

It is also available on the Unity asset store, and as such you can
enable Unity debug logging if you wish.

## Usage

### Parsing a `string` into a `JSONObject`

	using Boomlagoon.JSON;

	string text = "{ \"sample\" : 123 }";
	JSONObject json = JSONObject.Parse(text);
	double number = json.GetNumber("sample");


### Creating a `JSONObject`

	var obj = new JSONObject();
	obj.Add("key", "value");
	obj.Add("otherKey", 1234);
	obj.Add("bool", true);

	//Alternative method:
	var obj = new JSONObject {
		{"key", "value"}, 
		{"otherKey", 1234}, 
		{"bool", true}
	};


## Changelog

#### Version 1.3
+ Added the zlib license as the header of JSONObject.cs

#### Version 1.2
+ Replaced usage of Stack<T> with List<T> to remove dependency to system.dll as per Mike Weldon's suggestion.

* * *

## License

Boomlagoon JSON is licensed under the zlib License:

  Copyright (C) 2012 Boomlagoon Ltd.

  This software is provided 'as-is', without any express or implied
  warranty.  In no event will the authors be held liable for any damages
  arising from the use of this software.

  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

  1. The origin of this software must not be misrepresented; you must not
     claim that you wrote the original software. If you use this software
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.
  2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original software.
  3. This notice may not be removed or altered from any source distribution.

Boomlagoon Ltd.  
contact@boomlagoon.com