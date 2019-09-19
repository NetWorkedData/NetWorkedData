#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
@interface NWEUnitySingletonManager : NSObject {
    BOOL Ready;
    NSString *URLQueue;
}
@property BOOL Ready;
@property (nonatomic, retain) NSString *URLQueue;

+ (id)sharedInstance;
@end
