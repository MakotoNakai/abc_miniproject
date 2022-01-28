//
//  function.swift
//  abc_project_app
//
//  Created by 田中元 on 2022/01/27.
//

import Foundation
import CoreML

func sum(_ array:[Float]) -> Float {
        return array.reduce(0,+)
    }

func average(_ array:[Float]) -> Float {
    return sum(array) / Float(array.count)
}

func variance(_ array:[Float]) -> Float {
    let left=average(array.map{pow($0, 2.0)})
    let right=pow(average(array), 2.0)
    let count=array.count
    return (left-right) * Float(count/(count-1))
}

func abcCoreML(_ coreMotion: Array<Float>) -> [Float] {
    let modelURL = Bundle.main.url(forResource: "abc", withExtension: "mlmodelc")!
    let abcModel = try! MLModel(contentsOf: modelURL)
    guard let mlArray = try? MLMultiArray(shape: [1, 12], dataType: .double) else {
        fatalError("mlArray1")
    }
        
    for (index, element) in coreMotion.enumerated() {
        mlArray[index] = NSNumber(value: element)
    }

    let modelInput = abcInput(input_8: mlArray)
    guard let output = try? abcModel.prediction(from: modelInput) else {
        fatalError("The abc model is unable to make a prediction.")
    }
    
    let result = output.featureValue(for: "Identity")!.multiArrayValue!
    
    let buffer = try! UnsafeBufferPointer<Float>(result)
    let modelFloat = Array(buffer)
    return modelFloat
}

func judgeActivitiesType(_ motionList: [Float]) -> String {
    let idx = motionList.firstIndex(of: motionList.max()!)!
    let actions = ["sit", "walk", "steps"]
    let action = actions[idx]
    return action
}

func dataFormatToString() -> String {
    let dateFormatter = DateFormatter()
    dateFormatter.dateFormat = "yyyy-MM-dd'T'HH:mm:ssXXX"
    dateFormatter.locale = Locale(identifier: "en_US_POSIX")
    dateFormatter.timeZone = TimeZone(identifier: "Asia/Tokyo")
    return dateFormatter.string(from: Date())
}


func toJson(_ goods: JsonContents) -> Data {
       
    // オブジェクトからJsonに変換
    let encoder = JSONEncoder()
    guard let jsonValue = try? encoder.encode(goods) else {
       fatalError("Failed to encode to JSON.")
    }
    
       
    return jsonValue;
}

func httpPost(_ json: Data) {
    let config:URLSessionConfiguration = URLSessionConfiguration.default
    let url:NSURL = NSURL(string: "https://abcmini.warp-d.com/activity")!
    let request:NSMutableURLRequest = NSMutableURLRequest(url: url as URL)
    let session:URLSession = URLSession(configuration: config)

    request.httpMethod = "POST"

    // jsonのデータを一度文字列にして、キーと合わせる.
    let data:NSString = NSString(data: json as Data, encoding: String.Encoding.utf8.rawValue)! as NSString

    // jsonデータのセット.
    request.setValue("Application/json", forHTTPHeaderField: "Content-Type")
    request.httpBody = data.data(using: String.Encoding.utf8.rawValue)
    
    let _ = print(String(bytes: request.httpBody!, encoding: .utf8)!)

    let task:URLSessionDataTask = session.dataTask(with: request as URLRequest, completionHandler: { (_data, response, err) -> Void in
    })

    task.resume()
}
