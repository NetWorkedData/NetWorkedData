#import <Cocoa/Cocoa.h>
@interface NWEUnitySingletonManager : NSObject {
//    BOOL Ready;
//    NSString *URLQueue;
}
@property BOOL Ready;
@property (nonatomic, retain) NSString *URLQueue;

+ (id)sharedInstance;
-(void)handleAppleEvent:(NSAppleEventDescriptor *)event withReplyEvent:(NSAppleEventDescriptor *)replyEvent;
-(void)applicationDidFinishLaunching:(NSNotification *)notification;
@end
