//
//  ReminderViewController.swift
//  RemindMeNotes
//
//  Created by Danny Pires on 5/8/20.
//  Copyright © 2020 Danny Pires. All rights reserved.
//

import UserNotifications
import UIKit

class ReminderViewController: UIViewController, UITextFieldDelegate {
    
    //Three same necessary Outlets used for data of our reminder to be stored in Core Data
    @IBOutlet var txtFieldTitle: UITextField!
    @IBOutlet var txtFieldDesc: UITextField!
    @IBOutlet var datePicker: UIDatePicker!
    // Same as always uses the same save functionality.
    var completion: ((String, String, Date) -> Void)?
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.txtFieldTitle.delegate = self
        self .txtFieldDesc.delegate = self
        //same as Note and Entry
        navigationItem.rightBarButtonItem = UIBarButtonItem(title: "Save", style: .done, target: self, action: #selector(saveReminder))
    }
    
    @objc func saveReminder(){
        //Similar idea of checking that the fields are not empty
        if let remTitle = txtFieldTitle.text, !remTitle.isEmpty,
            let remDesc = txtFieldDesc.text, !remDesc.isEmpty {
            let remDate = datePicker.date
            
            completion?(remTitle, remDesc, remDate)
        }
    }
    //Pressing the return key in either fields allows the keyboard to be dropped.
    func textFieldShouldReturn(_ textField: UITextField) -> Bool {
        self.view.endEditing(true)
        return false
    }
}
