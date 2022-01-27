//
//  MotionSensor.swift
//  abc_miniproject
//
//  Created by 田中元 on 2022/01/27.
//

import Foundation
import CoreMotion

class MotionSensor: NSObject, ObservableObject {
    @Published var xList: Array<Float> = []
    @Published var yList: Array<Float> = []
    @Published var zList: Array<Float> = []
    @Published var mlList: Array<Float> = []
    
    let motionManager = CMMotionManager()
    
    func start() {
        self.reset()
        
        
        if motionManager.isDeviceMotionAvailable {
            motionManager.deviceMotionUpdateInterval = 0.1
            motionManager.startDeviceMotionUpdates(to: OperationQueue.current!, withHandler: {(motion:CMDeviceMotion?, error:Error?) in
                self.updateMotionData(deviceMotion: motion!)
            })
        }
    }
    
    func stop() {
        motionManager.stopDeviceMotionUpdates()
        self.calculate()

    }
    
    private func updateMotionData(deviceMotion:CMDeviceMotion) {
        xList.append(Float(deviceMotion.userAcceleration.x))
        yList.append(Float(deviceMotion.userAcceleration.y))
        zList.append(Float(deviceMotion.userAcceleration.z))
    }
    
    private func reset() {
        xList = []
        yList = []
        zList = []
        mlList = []
    }
    
    private func calculate() {
        if !xList.isEmpty {
            for list in [xList,yList,zList] {
                mlList.append(list.max()!)
                mlList.append(list.min()!)
                mlList.append(average(list))
                mlList.append(variance(list))
            }
        }
    }
}
