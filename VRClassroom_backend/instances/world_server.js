const Campus = require('./campus.js');
const Classroom = require('./classroom.js');

class WorldServer {
    ip          = "";
    instances   = 0;
    classrooms  = {};
    campuses    = {};

    constructor(ip){
        this.ip = ip;
    }

    static startingPort = 7778

    get newPort(){
        return WorldServer.startingPort + this.instances;
    }

    createCampus(ip, port, name){
        // Create new campus object and add it to the list
        let c = new Campus(ip, port, name);
        if(!this.campuses[name]){
            this.campuses[name] = [c];
        }else{
            this.campuses[name].push(c);
        }
        this.instances++;

        return c;
    }

    getCampus(name){
        // Return available campus
        if(this.campuses[name]){
            return this.campuses[name][0];
        }else{
        }
        return null;
    }

    createClassroom(ip, port, meeting_id){
        // Create new classroom object and add it to the dict
        let c = new Classroom(ip, port, meeting_id, "Physics");
        this.classrooms[meeting_id] = c;
        this.instances++;

        return c;
    }

    getClassroom(meeting_id){
        // Get correct classroom

        return this.classrooms[meeting_id];
    }
}

module.exports = WorldServer;