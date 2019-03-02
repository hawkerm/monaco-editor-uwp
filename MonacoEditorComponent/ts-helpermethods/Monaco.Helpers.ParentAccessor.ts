//namespace Monaco.Helpers {
    interface ParentAccessor {
        callAction(name: string): Promise<boolean>;
        callEvent(name: string, parameters: string[]): Promise<string>;
        close();
        getChildValue(name: string, child: string): Promise<any>;
        getJsonValue(name: string): Promise<string>;
        getValue(name: string): Promise<any>;
        setValue(name: string, value: any);
        setValue(name: string, value: string, type: string);
    }
//}