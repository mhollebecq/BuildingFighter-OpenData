var express = require('express');
var router = express.Router();

/* GET board page. */
router.get('/:insee', function(req, res, next) {
  res.render('board', { insee: req.params["insee"] });
});

module.exports = router;