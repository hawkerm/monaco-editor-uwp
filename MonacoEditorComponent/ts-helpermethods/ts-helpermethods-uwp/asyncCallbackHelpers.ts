﻿var callParentEventAsync = function (name: string, parameters: string[]): Promise<string> {
    return Parent.callEvent(name, parameters);
}

var callParentActionWithParameters = function (name: string, parameters: string[]): boolean {
    return Parent.callActionWithParameters(name, parameters);
}


