//
//  NWEHookBridge.m
//  NetWorkedDataOSX
//
//  Created by Jean-François CONTART on 07/06/2018.
//  Copyright © 2018 Jean-François CONTART. All rights reserved.
//
#import "NWEDialogOSXManager.h"
#include "NWEHookBridge.h"
//void CallMethod(const char *objectname, const char *commandName, const char *commandata);

@implementation NWEDialogOSXManager

static NSAlert* kCurrentAlert =  nil;

+(NSString *) charToNSString:(char *)sValue {
    if (sValue != NULL) {
        return [NSString stringWithUTF8String: sValue];
    } else {
        return [NSString stringWithUTF8String: ""];
    }
}
+(const char *)NSIntToChar:(NSInteger)sValue {
    NSString *tTmp = [NSString stringWithFormat:@"%ld", (long)sValue];
    return [tTmp UTF8String];
}
+ (const char *)NSStringToChar:(NSString *)sValue {
    return [sValue UTF8String];
}

+ (void) showDialog: (NSString *) title message: (NSString*) msg yesTitle:(NSString*) b1 noTitle: (NSString*) b2{
    NSLog(@"showDialog on mac");
    NSAlert *tAlert = [[NSAlert alloc] init];
    [tAlert setMessageText:title];
    [tAlert setInformativeText:msg];
    [tAlert addButtonWithTitle:b1];
    [tAlert addButtonWithTitle:b2];
    [tAlert setAlertStyle:NSAlertStyleWarning];
    NSModalResponse tResult = [tAlert runModal];
    if (tResult == NSAlertFirstButtonReturn) {
        NSLog(@"click on propositin one");
        SingletonSendMessage("NWEDialogOSX_GameObject", "OnDialogCallback",  [NWEDialogOSXManager NSIntToChar:0]);
    }
    else
    {
        NSLog(@" click on propositin two");
        SingletonSendMessage("NWEDialogOSX_GameObject", "OnDialogCallback",  [NWEDialogOSXManager NSIntToChar:1]);
    }
}
+ (void) showAlert: (NSString *) title message: (NSString*) msg okTitle:(NSString*) b1 {
    NSLog(@"showAlert on mac");
    NSAlert *tAlert = [[NSAlert alloc] init];
    [tAlert setMessageText:title];
    [tAlert setInformativeText:msg];
    [tAlert addButtonWithTitle:b1];
    [tAlert setAlertStyle:NSAlertStyleWarning];
    if ([tAlert runModal] == NSAlertFirstButtonReturn) {
        NSLog(@" click on propositin alert");
        SingletonSendMessage("NWEAlertOSX_GameObject", "OnAlertCallback",  [NWEDialogOSXManager NSIntToChar:0]);
    }
}

extern "C" {
    void _NWE_ShowDialog(char* title, char* message, char* ok, char* nok) {
        [NWEDialogOSXManager showDialog:[NWEDialogOSXManager charToNSString:title] message:[NWEDialogOSXManager charToNSString:message] yesTitle:[NWEDialogOSXManager charToNSString:ok] noTitle:[NWEDialogOSXManager charToNSString:nok]];
    }
    void _NWE_ShowAlert(char* title, char* message, char* ok) {
        [NWEDialogOSXManager showAlert:[NWEDialogOSXManager charToNSString:title] message:[NWEDialogOSXManager charToNSString:message] okTitle:[NWEDialogOSXManager charToNSString:ok]];
    }
}
@end
