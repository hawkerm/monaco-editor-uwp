//namespace Monaco.Helpers {
    interface ParentAccessor {
        callAction(name: string): boolean;
        callActionWithParameters(name: string, parameter1: string, parameter2: string): boolean;
        callEvent(name: string, callbackMethod: string, parameter1: string, parameter2: string);
        close();
        getChildValue(name: string, child: string): any;
        getJsonValue(name: string): string;
        getValue(name: string): any;
        setValue(name: string, value: any);
        setValue(name: string, value: string, type: string);
        //setValue(name: string, value: string);
        //setValueWithType(name: string, value: string, type: string);
    }
//}