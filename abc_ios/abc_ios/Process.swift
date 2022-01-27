//
//  Process.swift
//  abc_miniproject
//
//  Created by 田中元 on 2022/01/27.
//

import Foundation
import UIKit
import SwiftUI

class Process: ObservableObject {
    @ObservedObject var sensor: MotionSensor = MotionSensor()
    
    @Published var time = 0.0
    @Published var isStarted = false
    @Published var modelFloat:[Float]!
    @Published var deviceId: String = UIDevice.current.identifierForVendor!.uuidString
    @Published var type: String!
    @Published var timestamp: String!
    @Published var actibities: [String: String]!
    
    @Published var dataDict: JsonContents!
    @Published var jsonBody: Data!
    var timer = Timer()
    let defaultTimerCounter = 3
    var timerCounter:Int!
    
    func start() {
        self.timerCounter = self.defaultTimerCounter
        self.isStarted = true
        timer = Timer.scheduledTimer(withTimeInterval: 10.0, repeats: true, block: { (timer) in
            if self.timerCounter > 0 {
                self.sensor.start()
                DispatchQueue.main.asyncAfter(deadline: .now() + 5.0) {
                    self.stop()
                }
                self.timerCounter -= 1
            } else {
                self.timer.invalidate()
                self.isStarted = false
            }
        })
    }
    
    func stop() {
        self.sensor.stop()
        modelFloat = abcCoreML(self.sensor.mlList)
        
        type = judgeActivitiesType(modelFloat)
        timestamp = dataFormatToString()
        actibities = ["timestamp": timestamp, "type": type]

        dataDict = JsonContents(deviceId: deviceId, activities: [
            JsonContents.Activities(type: type, timestamp: timestamp)
        ])
        jsonBody =  toJson(dataDict)

        httpPost(jsonBody)
    }
    
    func exit() {
        self.timerCounter = 0
    }
}
