#import "NWDDialogIOSManager.h"

@implementation NWDDialogIOSManager

static UIAlertController* kCurrentAlert =  nil;
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
+ (void) unregisterAllertView {
    if(kCurrentAlert != nil) {
        kCurrentAlert = nil;
    }
}
+(void) dismissCurrentAlert {
    if(kCurrentAlert != nil) {
        [kCurrentAlert dismissViewControllerAnimated:NO completion:nil];
        kCurrentAlert = nil;
    }
}
+ (void) showDialog: (NSString *) title message: (NSString*) msg yesTitle:(NSString*) b1 noTitle: (NSString*) b2{
    UIAlertController *alertController = [UIAlertController alertControllerWithTitle:title message:msg preferredStyle:UIAlertControllerStyleAlert];
    UIAlertAction *yesAction = [UIAlertAction actionWithTitle:b1 style:UIAlertActionStyleDefault handler:^(UIAlertAction * _Nonnull action) {
        UnitySendMessage("NWDDialogIOS_GameObject", "OnDialogCallback",  [NWDDialogIOSManager NSIntToChar:0]);
        [NWDDialogIOSManager unregisterAllertView];
    }];
    UIAlertAction *noAction = [UIAlertAction actionWithTitle:b2 style:UIAlertActionStyleDefault handler:^(UIAlertAction * _Nonnull action) {
        UnitySendMessage("NWDDialogIOS_GameObject", "OnDialogCallback",  [NWDDialogIOSManager NSIntToChar:1]);
        [NWDDialogIOSManager unregisterAllertView];
    }];
    [alertController addAction:yesAction];
    [alertController addAction:noAction];
    [[[[UIApplication sharedApplication] keyWindow] rootViewController] presentViewController:alertController animated:YES completion:nil];
    kCurrentAlert = alertController;
}
+(void)showAlert: (NSString *) title message: (NSString*) msg okTitle:(NSString*) b1 {
    UIAlertController *alertController = [UIAlertController alertControllerWithTitle:title message:msg preferredStyle:UIAlertControllerStyleAlert];
    UIAlertAction *okAction = [UIAlertAction actionWithTitle:b1 style:UIAlertActionStyleDefault handler:^(UIAlertAction * _Nonnull action) {
        UnitySendMessage("NWDAlertIOS_GameObject", "OnAlertCallback",  [NWDDialogIOSManager NSIntToChar:0]);
        [NWDDialogIOSManager unregisterAllertView];
    }];
    [alertController addAction:okAction];
    [[[[UIApplication sharedApplication] keyWindow] rootViewController] presentViewController:alertController animated:YES completion:nil];
    kCurrentAlert = alertController;
}

extern "C" {
    void _NWD_ShowDialog(char* title, char* message, char* yes, char* no) {
        [NWDDialogIOSManager showDialog:[NWDDialogIOSManager charToNSString:title] message:[NWDDialogIOSManager charToNSString:message] yesTitle:[NWDDialogIOSManager charToNSString:yes] noTitle:[NWDDialogIOSManager charToNSString:no]];
    }
    void _NWD_ShowAlert(char* title, char* message, char* ok) {
        [NWDDialogIOSManager showAlert:[NWDDialogIOSManager charToNSString:title] message:[NWDDialogIOSManager charToNSString:message] okTitle:[NWDDialogIOSManager charToNSString:ok]];
    }
    void _NWD_DismissAlert() {
        [NWDDialogIOSManager dismissCurrentAlert];
    }
}
@end
