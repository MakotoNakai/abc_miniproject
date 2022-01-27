//
//  ContentView.swift
//  abc_project_app
//
//  Created by 田中元 on 2022/01/27.
//

import SwiftUI
import UIKit


struct ContentView: View {
    @ObservedObject var process = Process()
    var body: some View {
        VStack {
            Text("device ID : " + String(process.deviceId)).padding(.bottom)
            Button(action: {
                UIPasteboard.general.string = process.deviceId
            }) {
                Text("copy device ID").foregroundColor(Color.blue)
            }.padding(.all).overlay(RoundedRectangle(cornerRadius: 10).stroke(Color.blue, lineWidth: 1))
            
            Button(action: {
                self.process.isStarted ? self.process.timerCounter = 0 : self.process.start()
            }) {
                (self.process.isStarted ? Text("STOP") : Text("START")).foregroundColor(Color.blue)
            }.padding(.all)
            .overlay(RoundedRectangle(cornerRadius: 10)
            .stroke(Color.blue, lineWidth: 1))
            .padding(EdgeInsets(
                top: 50,
                leading: 0,
                bottom: 0,
                trailing: 0
            ))
        }
    }
}

struct ContentView_Previews: PreviewProvider {
    static var previews: some View {
        ContentView()
    }
}




//https://yukblog.net/core-motion-basics/

