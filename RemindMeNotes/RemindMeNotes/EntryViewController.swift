//
//  EntryViewController.swift
//  RemindMeNotes
//
//  Created by Danny Pires on 5/4/20.
//  Copyright © 2020 Danny Pires. All rights reserved.
//

import UIKit

class EntryViewController: UIViewController {
    
    //Outlets for the two necessary fields
    @IBOutlet var txtFieldTitle: UITextField!
    @IBOutlet var txtViewNote: UITextView!
    //same complete from the NoteViewController
    var complete: ((String,String)->Void)?
    
    override func viewDidLoad() {
        super.viewDidLoad()
        txtFieldTitle.becomeFirstResponder()
        //Create save button that calls selSave an objc function to save date entered into the fields
        navigationItem.rightBarButtonItem = UIBarButtonItem(title: "Save", style: .done, target: self, action: #selector(selSave))
    }
    
    //Same functionality as the NoteViewController
    @objc func selSave() {
        if let text = txtFieldTitle.text, !text.isEmpty, !txtViewNote.text.isEmpty {
            complete?(text, txtViewNote.text)
        }
    }
    
}
