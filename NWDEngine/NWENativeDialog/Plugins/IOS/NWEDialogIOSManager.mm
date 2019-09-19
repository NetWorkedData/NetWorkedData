#import "NWEDialogIOSManager.h"

@implementation NWEDialogIOSManager

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
        UnitySendMessage("NWEDialogIOS_GameObject", "OnDialogCallback",  [NWEDialogIOSManager NSIntToChar:0]);
        [NWEDialogIOSManager unregisterAllertView];
    }];
    UIAlertAction *noAction = [UIAlertAction actionWithTitle:b2 style:UIAlertActionStyleDefault handler:^(UIAlertAction * _Nonnull action) {
        UnitySendMessage("NWEDialogIOS_GameObject", "OnDialogCallback",  [NWEDialogIOSManager NSIntToChar:1]);
        [NWEDialogIOSManager unregisterAllertView];
    }];
    [alertController addAction:yesAction];
    [alertController addAction:noAction];
    [[[[UIApplication sharedApplication] keyWindow] rootViewController] presentViewController:alertController animated:YES completion:nil];
    kCurrentAlert = alertController;
}
+(void)showAlert: (NSString *) title message: (NSString*) msg okTitle:(NSString*) b1 {
    UIAlertController *alertController = [UIAlertController alertControllerWithTitle:title message:msg preferredStyle:UIAlertControllerStyleAlert];
    UIAlertAction *okAction = [UIAlertAction actionWithTitle:b1 style:UIAlertActionStyleDefault handler:^(UIAlertAction * _Nonnull action) {
        UnitySendMessage("NWEAlertIOS_GameObject", "OnAlertCallback",  [NWEDialogIOSManager NSIntToChar:0]);
        [NWEDialogIOSManager unregisterAllertView];
    }];
    [alertController addAction:okAction];
    [[[[UIApplication sharedApplication] keyWindow] rootViewController] presentViewController:alertController animated:YES completion:nil];
    kCurrentAlert = alertController;
}

extern "C" {
    void _NWE_ShowDialog(char* title, char* message, char* yes, char* no) {
        [NWEDialogIOSManager showDialog:[NWEDialogIOSManager charToNSString:title] message:[NWEDialogIOSManager charToNSString:message] yesTitle:[NWEDialogIOSManager charToNSString:yes] noTitle:[NWEDialogIOSManager charToNSString:no]];
    }
    void _NWE_ShowAlert(char* title, char* message, char* ok) {
        [NWEDialogIOSManager showAlert:[NWEDialogIOSManager charToNSString:title] message:[NWEDialogIOSManager charToNSString:message] okTitle:[NWEDialogIOSManager charToNSString:ok]];
    }
    void _NWE_DismissAlert() {
        [NWEDialogIOSManager dismissCurrentAlert];
    }
}
@end
