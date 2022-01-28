//
//  JasonContents.swift
//  abc_miniproject
//
//  Created by 田中元 on 2022/01/27.
//

import Foundation

struct JsonContents: Codable {
    var deviceId: String
    var activities: [Activities]
    
    struct Activities: Codable {
        var type: String
        var timestamp: String
    }
}
