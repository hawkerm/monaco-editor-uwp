declare var Parent: ParentAccessor;

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
        promise(parameter);
        //if (parameter) {
        //    var resultObject = JSON.parse(parameter);
        //    console.log(">>>> Parameter decoded to object <<<<<<");
        //    promise(resultObject);
        //}
        //else {
        //    console.log(">>>> Parameter is null <<<<<<");
        //    promise(parameter);
        //}
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
    return sanitize(value);
}

var callParentEventAsync = function (name: string, parameter1: string, parameter2: string): Promise<string>  {
    return invokeAsyncMethod<string>((promiseId) => Parent.callEvent(name, promiseId, stringifyForMarshalling(parameter1), stringifyForMarshalling(parameter2))).then(result => {
        if (result) {
            return JSON.parse(result);
        }
    });
}

var callParentActionWithParameters = function (name: string, parameters: string[]): boolean {
    return Parent.callActionWithParameters(name,
        parameters.length > 0 ? stringifyForMarshalling(parameters[0]) : null,
        parameters.length > 1 ? stringifyForMarshalling(parameters[1]) : null);

}
