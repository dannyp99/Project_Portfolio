const WorldServer   = require('./world_server.js');
const fetch         = require('node-fetch');

const servers = {};
let controller;
class InstancesController {

    addWorldServer(ctx) {

        const ip = ctx.request.headers["x-orig-ip"];
        let s = new WorldServer(ip);
        servers[ip] = s;

        console.log(`Adding Server ${ip}`);

        ctx.response.status = 204;
    }

    async joinServer(ctx) {
        let campus = controller.getCampus("ASD",'Hofstra');
        if(!campus){
            campus = await controller.createCampus('Hofstra');
        }
        if(!campus){
            ctx.response.status = 400;
        }else{
            ctx.response.status = 200;
            ctx.response.body = {
                ip: campus.ip,
                port: `${campus.port}`
            };;
        }
    }

    async joinMeeting(ctx) {
        const meeting_id = ctx.params.id;
        let classroom = controller.getClassroom("Name", meeting_id);
        if(!classroom) {
            classroom = await controller.createClassroom(meeting_id);
        }
        if(!classroom) {
            ctx.response.status = 400;
        }else {
            ctx.response.status = 200;
            ctx.response.body = {
                ip: classroom.ip,
                port: `${classroom.port}`
            }
        }
    }

    async createCampus(campus_name) {
        let s = null;
        for(let key in servers){
            s = servers[key];
        }
        if(!s){
            console.log('No world server');
            return;
        }
        const body = {
            name: "TestServer",
            port: s.newPort
        }
        const response = await fetch(`http://${s.ip}:8181/ue4`, {
            method: 'POST',
            body: JSON.stringify(body),
            headers: {
                'Content-Type': 'application/json'
            }
        });
        if(!response.ok){
            return null;
        }
        return s.createCampus(s.ip, s.newPort, campus_name);
    }

    getCampus(player, campus_name) {
        if(servers.length === 0){
            return null;
        }
        let campus = null;
        for(let key in servers){
            campus = servers[key].getCampus(campus_name);
            if(campus){
                break;
            }
        }

        return campus;
    }

    async createClassroom(meeting_id) {
        let s = null;
        for(let key in servers){
            s = servers[key];
        }
        if(!s){
            console.log('No world server');
            return;
        }
        const body = {
            name: "TestClassroom",
            port: s.newPort
        }
        const response = await fetch(`http://${s.ip}:8181/ue4/classroom`, {
            method: 'POST',
            body: JSON.stringify(body),
            headers: {
                'Content-Type': 'application/json'
            }
        });
        if(!response.ok){
            return null;
        }
        return s.createClassroom(s.ip, s.newPort, meeting_id);
    }

    getClassroom(player, meeting_id) {
        if(servers.length === 0){
            return null;
        }
        let campus = null;
        for(let key in servers){
            campus = servers[key].getClassroom(meeting_id);
            if(campus){
                break;
            }
        }
        return campus;
    }
}

controller = new InstancesController();

module.exports = controller;
