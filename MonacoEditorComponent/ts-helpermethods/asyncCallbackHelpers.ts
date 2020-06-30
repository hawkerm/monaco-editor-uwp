
// TODO: Fix build issue related to Promise
declare var Promise: any;

type NumberCallback = (parameter: any) => void;
declare var asyncCallbackMap: { [promiseId: string]: NumberCallback };
declare var nextAsync: number;

nextAsync = 1;
asyncCallbackMap = {};

var asyncCallback = function (promiseId: string, parameter: string) {
    console.log(">>>> Promise Id ${promiseId}<<<<<<");
    console.log(">>>> Parameter ${parameter}<<<<<<");
    var promise = asyncCallbackMap[promiseId];
    if (promise) {
        console.log(">>>> Promise found <<<<<<");
        if (parameter) {
            var resultObject = JSON.parse(parameter);
            console.log(">>>> Parameter decoded to object <<<<<<");
            promise(resultObject);
        }
        else {
            console.log(">>>> Parameter is null <<<<<<");
            promise(parameter);
        }
        console.log(">>>> Promise resolved <<<<<<");

    }
    console.log(">>>> Async callback <<<<<<");
}


var invokeAsyncMethod = function <T> (syncMethod: NumberCallback) : Promise<T>
{
    if (nextAsync==null) {
        nextAsync = 0;
    }
    if (asyncCallbackMap==null) {
        asyncCallbackMap = {};
    }


    var promise = new Promise<T>((resolve, reject) => {
        var nextId = nextAsync++;
        asyncCallbackMap[nextId] = resolve;
        syncMethod(nextId?.toString());
    });
    return promise;
}

var replaceAll = function (str: string, find: string, rep: string): string {
    return str.replace(new RegExp(find, 'g'), rep);
}

var sanitize = function (jsonString: string): string {
    if (jsonString == null) return null;

    var replacements = "&\"'{}:,";
    for (var i = 0; i < replacements.length; i++) {
        jsonString = replaceAll(jsonString, replacements.charAt(i), "%" + replacements.charCodeAt(i));
    }
    return jsonString;
}

var stringifyForMarshalling=function (value: any): string {
    return sanitize(JSON.stringify(value));
}
