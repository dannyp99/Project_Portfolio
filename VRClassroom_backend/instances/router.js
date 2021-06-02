const InstancesController  = require('./controller');
const Router               = require('@koa/router');
const publicRouter         = new Router();
const protectedRouter      = new Router();

publicRouter.get('/add-world-server', InstancesController.addWorldServer);

protectedRouter.get('/join-server', InstancesController.joinServer);

protectedRouter.get('/join-classroom/:meetingId', InstancesController.joinMeeting);

module.exports = [publicRouter, protectedRouter];