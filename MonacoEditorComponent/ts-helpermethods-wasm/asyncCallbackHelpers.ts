declare var Parent: ParentAccessor;
declare var Theme: ThemeAccessor;

type MethodWithReturnId = (parameter: string) => void;
type NumberCallback = (parameter: any) => void;
declare var asyncCallbackMap: { [promiseId: string]: NumberCallback };
declare var nextAsync: number;

nextAsync = 1;
asyncCallbackMap = {};

declare var returnValueCallbackMap: { [returnId: string]: string };
declare var nextReturn: number;

nextReturn = 1;
returnValueCallbackMap = {};

const asyncCallback = (promiseId: string, parameter: string) => {
    const promise = asyncCallbackMap[promiseId];
    if (promise) {
        //console.log('Async response: ' + parameter);
        promise(parameter);
    }
}

const returnValueCallback = (returnId: string, returnValue: string) => {
    //console.log('Return value for id ' + returnId + ' is ' + returnValue);
    returnValueCallbackMap[returnId] = returnValue;
}

const invokeAsyncMethod = <T>(syncMethod: NumberCallback): Promise<T> => {
    if (nextAsync==null) {
        nextAsync = 0;
    }
    if (asyncCallbackMap==null) {
        asyncCallbackMap = {};
    }
    const promise = new Promise<T>((resolve, reject) => {
        var nextId = nextAsync++;
        asyncCallbackMap[nextId] = resolve;
        syncMethod(`${nextId}`);
    });
    return promise;
}

const replaceAll = (str: string, find: string, rep: string): string => {
    if (find == "\\")
    {
        find = "\\\\";
    }
    return (`${str}`).replace(new RegExp(find, "g"), rep);
}

const sanitize = (jsonString: string): string => {
    if (jsonString == null) {
        //console.log('Sanitized is null');
        return null;
    }

    const replacements = "%&\\\"'{}:,";
    for (let i = 0; i < replacements.length; i++) {
        jsonString = replaceAll(jsonString, replacements.charAt(i), `%${replacements.charCodeAt(i)}`);
    }
    //console.log('Sanitized: ' + jsonString);
    return jsonString;
}

const desantize = (parameter: string): string => {
    //System.Diagnostics.Debug.WriteLine($"Encoded String: {parameter}");
    if (parameter == null) return parameter;
    const replacements = "&\\\"'{}:,%";
    //System.Diagnostics.Debug.WriteLine($"Replacements: >{replacements}<");
    for (let i = 0; i < replacements.length; i++)
    {
        //console.log("Replacing: >%" + replacements.charCodeAt(i) + "< with >" + replacements.charAt(i) + "< ");
        parameter = replaceAll(parameter, "%" + replacements.charCodeAt(i), replacements.charAt(i));
    }

    //console.log("Decoded String: " + parameter );
    return parameter;
}

const stringifyForMarshalling = (value: any): string => sanitize(value)

const invokeWithReturnValue = (methodToInvoke: MethodWithReturnId): string => {
    const nextId = nextReturn++;
    methodToInvoke(nextId + '');
    var json = returnValueCallbackMap[nextId];
    //console.log('Return json ' + json);
    json = desantize(json);
    return json;
}

const getParentValue = (name: string): any => {
    const jsonString = invokeWithReturnValue((returnId) => Parent.getJsonValue(name, returnId));
    const obj = JSON.parse(jsonString);
    return obj;
}

const getParentJsonValue = (name: string): string =>
    invokeWithReturnValue((returnId) => Parent.getJsonValue(name, returnId))

const getThemeIsHighContrast = (): boolean =>
    invokeWithReturnValue((returnId) => Theme.getIsHighContrast(returnId)) == "true";

const getThemeCurrentThemeName = (): string =>
    invokeWithReturnValue((returnId) => Theme.getCurrentThemeName(returnId));


const callParentEventAsync = (name: string, parameters: string[]): Promise<string> =>
    invokeAsyncMethod<string>(async (promiseId) => {
        let result = await Parent.callEvent(name,
            promiseId,
            parameters != null && parameters.length > 0 ? stringifyForMarshalling(parameters[0]) : null,
            parameters != null && parameters.length > 1 ? stringifyForMarshalling(parameters[1]) : null);
        if (result) {
            //console.log('Parent event result: ' + name + ' -  ' +  result);
            result = desantize(result);
            //console.log('Desanitized: ' + name + ' -  ' + result);
        } else {
            //console.log('No Parent event result for ' + name);
        }

        return result;
    });

const callParentActionWithParameters = (name: string, parameters: string[]): boolean =>
    Parent.callActionWithParameters(name,
        parameters != null && parameters.length > 0 ? stringifyForMarshalling(parameters[0]) : null,
        parameters != null && parameters.length > 1 ? stringifyForMarshalling(parameters[1]) : null);
