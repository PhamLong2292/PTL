//Những toán tử dùng trong elasticsearch
var operators = new Object();
operators.not_eq = "not_eq";
operators.gt = "gt";
operators.gte = "gte";
operators.lt = "lt";
operators.lte = "lte";
operators.term = "term";
operators.not_term = "not_term";
operators.match = "match";
operators.not_match = "not_match";
operators.exists = "exists";
operators.not_exists = "not_exists";
//Nơi viết các hàm javascript để dùng chung cho toàn hệ thống
//Hàm CallInitSelect2
function CallInitSelect2(Id, svUrl,place)
{
    $('#' + Id).select2({
        width: '100%', 
        allowClear: true,
        placeholder:place,
        ajax: { 
            id: function (e) { return e.id},  
            url: svUrl, 
            dataType: 'json', 
            delay: 250, 
            type: 'POST',  
            data: function (params) { 
                return { 
                    q: params.term,  
                    page: params.page 
                }; 
            }, 
            processResults: function (data, params) { 
                params.page = params.page || 0; 
                console.log(params.page); 
                return { 
                    results: data.items, 
                    pagination: { 
                        more: (params.page * 20 + data.items.length) < data.total_count

                    } 
                }; 
            }, 
            cache: true 
        }, 
        escapeMarkup: function (markup) { return markup; },  
        minimumInputLength: 0, 
        templateResult: function (repo) {
            var markup = '';
            if (repo.loading) return repo.text;
            else if (repo.id == null) markup = '<table style="width: 100%;border-bottom: 1px solid black;"><tr><td style="width:20%;padding:4px; text-align: left;"><h3>' + repo.Code + '</h3></td> <td style=\"text-align: left;\"><h3>' + repo.Name + '</h3></td></tr></table>';
            else markup = '<table style="width: 100%;"><tr><td style="color:maroon;font-weight:bold; width:20%;padding:4px; text-align: left;">' + repo.Code + '</td> <td style=\"text-align: left;\">' + repo.Name + '</td></tr></table>';
            return markup;
        },  
        templateSelection: function (repo) {
            if(repo.id == "" || repo.Code == undefined) return repo.text;
            return repo.Code + " - " + repo.Name;
        }  
    }); 
}
function CallInitSelect2X(selector, svUrl,place)
{
    $(selector).select2({
        width: '100%', 
        allowClear: true,
        placeholder:place,
        ajax: { 
            id: function (e) { return e.id},  
            url: svUrl, 
            dataType: 'json', 
            delay: 250, 
            type: 'POST',  
            data: function (params) { 
                return { 
                    q: params.term,  
                    page: params.page 
                }; 
            }, 
            processResults: function (data, params) { 
                params.page = params.page || 0; 
                console.log(params.page); 
                return { 
                    results: data.items, 
                    pagination: { 
                        more: (params.page * 20 + data.items.length) < data.total_count

                    } 
                }; 
            }, 
            cache: true 
        }, 
        escapeMarkup: function (markup) { return markup; },  
        minimumInputLength: 0, 
        templateResult: function (repo) {
            var markup = '';
            if (repo.loading) return repo.text;
            else if (repo.id == null) markup = '<table style="width: 100%;border-bottom: 1px solid black;"><tr><td style="width:20%;padding:4px; text-align: left;"><h3>' + repo.Code + '</h3></td> <td style=\"text-align: left;\"><h3>' + repo.Name + '</h3></td></tr></table>';
            else markup = '<table style="width: 100%;"><tr><td style="color:maroon;font-weight:bold; width:20%;padding:4px; text-align: left;">' + repo.Code + '</td> <td style=\"text-align: left;\">' + repo.Name + '</td></tr></table>';
            return markup;
        },  
        templateSelection: function (repo) {
            if(repo.id == "" || repo.Code == undefined) return repo.text;
            return repo.Code + " - " + repo.Name;
        }  
    }); 
}
//CallInitSelect2 chỉ hiển thị tên trên combobox
function CallInitSelect2ForName(Id, svUrl,place)
{
    $('#' + Id).select2({
        width: '100%',
        allowClear: true,
        placeholder:place,
        ajax: { 
            //          id: function (e) { return e.id},  
            url: svUrl, 
            dataType: 'json', 
            delay: 250, 
            type: 'POST',
            data: function (params) { 
                return { 
                    q: params.term,  
                    page: params.page 
                }; 
            }, 
            processResults: function (data, params) { 
                params.page = params.page || 0; 
                console.log(params.page); 
                return { 
                    results: data.items, 
                    pagination: {
                        more: (params.page * 20 + data.items.length) < data.total_count
                    } 
                }; 
            }, 
            cache: true 
        }, 
        escapeMarkup: function (markup) { return markup; },  
        minimumInputLength: 0, 
        templateResult: function (repo) {
            var markup = '';
            if (repo.loading) return repo.text;
            else if (repo.id == null) markup = '<table style="width: 100%;border-bottom: 1px solid black;"><tr><td style="width:20%;padding:4px; text-align: left;"><h3>' + repo.Code + '</h3></td> <td style=\"text-align: left;\"><h3>' + repo.Name + '</h3></td></tr></table>';
            else markup = '<table style="width: 100%;"><tr><td style="color:maroon;font-weight:bold; width:20%;padding:4px; text-align: left;">' + repo.Code + '</td> <td style=\"text-align: left;\">' + repo.Name + '</td></tr></table>';
            return markup;
        },  
        templateSelection: function (repo) {
            if(repo.id == "" || repo.Code == undefined) return repo.text;
            return repo.Name;
        } 
    }); 
}

//Hàm CallInitSelect2 truy vấn dữ liệu cơ bản từ Elasticsearch cho những index có trường hieuluc, ma, ten (thường là các bảng danh mục)
function CallInitSelect2El(Id, svUrl, place)
{
    $('#' + Id).select2({
        width: '100%', 
        allowClear: true,
        placeholder:place,
        ajax: { 
            id: function (e) { return e.id},  
            url: svUrl, 
            //dataType: 'application/json', 
            delay: 250, 
            type: 'POST',  
            headers: { 'Content-Type': 'application/json' },
            //params:{CodeField: codeField},
            data: function (params) {              
                params.page = params.page || 0;
                if(params.term == undefined)
                    return JSON.stringify({
                        "query": {
                            "bool": {
                                "must": [
                                  {
                                      "match": {
                                          "hieuLuc": 1
                                      }
                                  }
                                ]
                            }
                        }
                        ,"size": 20
                        , "from": params.page * 20
                    });
                else 
                    return JSON.stringify({
                        "query": {
                            "bool": {
                                "must": [
                                  {
                                      "match": {
                                          "hieuLuc": 1
                                      }
                                  },
                                  {
                                      "bool": {
                                          "should": [
                                            {
                                                "match_phrase_prefix" : {
                                                    "ma": {
                                                        "query": params.term,
                                                        "slop": 3
                                                    }
                                                }
                                            },
                                            {
                                                "match_phrase_prefix" : {
                                                    "ten": {
                                                        "query": params.term,
                                                        "slop": 3
                                                    }
                                                }
                                            },
                                            {
                                                "wildcard": {
                                                    "ma": "*" + params.term + "*"
                                                }
                                            },
                                            {
                                                "wildcard": {
                                                    "ten": "*" + params.term + "*"
                                                }
                                            }
                                          ]
                                      }
                                  }
                                ]
                            }
                        }
                      ,"_source": ["id", "ma", "ten" ]
                      ,"size": 20
                      , "from": params.page * 20
                    });
            }, 
            processResults: function (data, params) { 
                params.page = params.page || 0; 
                console.log(params.page); 
                var results = [];
                $.each(data.hits.hits, function (index, item) { results.push({ id: item._id, ma: item._source.ma, ten: item._source.ten, text: item._source.ten }); });
                return { 
                    results: results, 
                    pagination: { 
                        more: (params.page * 20 + data.hits.hits.length) < data.hits.total.value
                    } 
                };
            }, 
            cache: true 
        }, 
        escapeMarkup: function (markup) { 
            return markup; 
        },  
        minimumInputLength: 0, 
        templateResult: function (repo) {
            var markup = '';
            if (repo.loading) return repo.text;
            else if (repo.id == null) markup = '<table style="width: 100%;border-bottom: 1px solid black;"><tr><td style="width:40%;padding:4px; text-align: left;"><h3>' + repo.ma + '</h3></td> <td style=\"text-align: left;\"><h3>' + repo.ten + '</h3></td></tr></table>';
            else markup = '<table style="width: 100%;"><tr><td style="color:maroon;font-weight:bold; width:40%;padding:4px; text-align: left;">' + repo.ma + '</td> <td style=\"text-align: left;\">' + repo.ten + '</td></tr></table>';
            return markup;
        },  
        templateSelection: function (repo) {
            if(repo.ma == undefined) return repo.text;
            return '(' + repo.ma + ") " + repo.ten;
        } 
    }); 
}
function CallInitSelect2ElC(selector, svUrl, place)
{
    $(selector).select2({
        width: '100%', 
        allowClear: true,
        placeholder:place,
        ajax: { 
            id: function (e) { return e.id},  
            url: svUrl, 
            //dataType: 'application/json', 
            delay: 250, 
            type: 'POST',  
            headers: { 'Content-Type': 'application/json' },
            //params:{CodeField: codeField},
            data: function (params) {              
                params.page = params.page || 0;
                if(params.term == undefined)
                    return JSON.stringify({
                        "query": {
                            "bool": {
                                "must": [
                                  {
                                      "match": {
                                          "hieuLuc": 1
                                      }
                                  }
                                ]
                            }
                        }
                        ,"size": 20
                        , "from": params.page * 20
                    });
                else 
                    return JSON.stringify({
                        "query": {
                            "bool": {
                                "must": [
                                  {
                                      "match": {
                                          "hieuLuc": 1
                                      }
                                  },
                                  {
                                      "bool": {
                                          "should": [
                                            {
                                                "match_phrase_prefix" : {
                                                    "ma": {
                                                        "query": params.term,
                                                        "slop": 3
                                                    }
                                                }
                                            },
                                            {
                                                "match_phrase_prefix" : {
                                                    "ten": {
                                                        "query": params.term,
                                                        "slop": 3
                                                    }
                                                }
                                            },
                                            {
                                                "wildcard": {
                                                    "ma": "*" + params.term + "*"
                                                }
                                            },
                                            {
                                                "wildcard": {
                                                    "ten": "*" + params.term + "*"
                                                }
                                            }
                                          ]
                                      }
                                  }
                                ]
                            }
                        }
                      ,"_source": ["id", "ma", "ten" ]
                      ,"size": 20
                      , "from": params.page * 20
                    });
            }, 
            processResults: function (data, params) { 
                params.page = params.page || 0; 
                console.log(params.page); 
                var results = [];
                $.each(data.hits.hits, function (index, item) { results.push({ id: item._id, ma: item._source.ma, ten: item._source.ten, text: item._source.ten }); });
                return { 
                    results: results, 
                    pagination: { 
                        more: (params.page * 20 + data.hits.hits.length) < data.hits.total.value
                    } 
                };
            }, 
            cache: true 
        }, 
        escapeMarkup: function (markup) { 
            return markup; 
        },  
        minimumInputLength: 0, 
        templateResult: function (repo) {
            var markup = '';
            if (repo.loading) return repo.text;
            else if (repo.id == null) markup = '<table style="width: 100%;border-bottom: 1px solid black;"><tr><td style="width:40%;padding:4px; text-align: left;"><h3>' + repo.ma + '</h3></td> <td style=\"text-align: left;\"><h3>' + repo.ten + '</h3></td></tr></table>';
            else markup = '<table style="width: 100%;"><tr><td style="color:maroon;font-weight:bold; width:40%;padding:4px; text-align: left;">' + repo.ma + '</td> <td style=\"text-align: left;\">' + repo.ten + '</td></tr></table>';
            return markup;
        },  
        templateSelection: function (repo) {
            if(repo.ma == undefined) return repo.text;
            return '(' + repo.ma + ") " + repo.ten;
        } 
    }); 
}
// Custom EL for UPPERCASE
function CallInitSelect2ElX(selector, svUrl, place)
{
    $(selector).select2({
        width: '100%', 
        allowClear: true,
        placeholder:place,
        ajax: { 
            id: function (e) { return e.id},  
            url: svUrl, 
            //dataType: 'application/json', 
            delay: 250, 
            type: 'POST',  
            headers: { 'Content-Type': 'application/json' },
            //params:{CodeField: codeField},
            data: function (params) {              
                params.page = params.page || 0;
                if(params.term == undefined)
                    return JSON.stringify({
                        "query": {
                            "bool": {
                                "must": [
                                  {
                                      "match": {
                                          "hIEULUC": 1
                                      }
                                  }
                                ]
                            }
                        }
                        ,"size": 20
                        , "from": params.page * 20
                    });
                else 
                    return JSON.stringify({
                        "query": {
                            "bool": {
                                "must": [
                                  {
                                      "match": {
                                          "hIEULUC": 1
                                      }
                                  },
                                  {
                                      "bool": {
                                          "should": [
                                            {
                                                "match_phrase_prefix" : {
                                                    "mA": {
                                                        "query": params.term,
                                                        "slop": 3
                                                    }
                                                }
                                            },
                                            {
                                                "match_phrase_prefix" : {
                                                    "tEN": {
                                                        "query": params.term,
                                                        "slop": 3
                                                    }
                                                }
                                            },
                                            {
                                                "wildcard": {
                                                    "mA": "*" + params.term + "*"
                                                }
                                            },
                                            {
                                                "wildcard": {
                                                    "tEN": "*" + params.term + "*"
                                                }
                                            }
                                          ]
                                      }
                                  }
                                ]
                            }
                        }
                      ,"_source": ["iD", "mA", "tEN" ]
                      ,"size": 20
                      , "from": params.page * 20
                    });
            }, 
            processResults: function (data, params) { 
                params.page = params.page || 0; 
                console.log(params.page); 
                var results = [];
                $.each(data.hits.hits, function (index, item) { results.push({ id: item._id, ma: item._source.mA, ten: item._source.tEN, text: item._source.tEN }); });
                return { 
                    results: results, 
                    pagination: { 
                        more: (params.page * 20 + data.hits.hits.length) < data.hits.total.value
                    } 
                };
            }, 
            cache: true 
        }, 
        escapeMarkup: function (markup) { 
            return markup; 
        },  
        minimumInputLength: 0, 
        templateResult: function (repo) {
            var markup = '';
            if (repo.loading) return repo.text;
            else if (repo.id == null) markup = '<table style="width: 100%;border-bottom: 1px solid black;"><tr><td style="width:40%;padding:4px; text-align: left;"><h3>' + repo.ma + '</h3></td> <td style=\"text-align: left;\"><h3>' + repo.ten + '</h3></td></tr></table>';
            else markup = '<table style="width: 100%;"><tr><td style="color:maroon;font-weight:bold; width:40%;padding:4px; text-align: left;">' + repo.ma + '</td> <td style=\"text-align: left;\">' + repo.ten + '</td></tr></table>';
            return markup;
        },  
        templateSelection: function (repo) {
            if(repo.ma == undefined) return repo.text;
            return '(' + repo.ma + ") " + repo.ten;
        } 
    }); 
}
// Hàm lấy owneruser
function CallInitSelect2El_OwnerUser(Id, svUrl, place,bsdd=0)
{
 $("#"+Id).select2({ 
       placeholder: place,
       allowClear: true,
       ajax: { 
           id: function (e) { return e.id + ' ' + e.text },  
           url: svUrl,
           dataType: 'json', 
           delay: 250, 
           type: 'POST',  
           headers: { 'Content-Type': 'application/json' },
           cache: true, 
           data: function (params) {
               params.page = params.page || 0;
               var jsondata = { query: { bool : { filter : [ { term : { active : 1 } } ] } }, from : (params.page * 25), size : 25 };
               if(bsdd == 1)
                   jsondata = { query: { bool : { must : [ { match : { active : 1 } },{bool: {should : [{ match : { bacSi : true } },{ match : { dieuDuong : true } }]}} ] } }, from : (params.page * 25), size : 25 };
               else if(bsdd == 2)
                   jsondata = { query: { bool : { must : [ { match : { active : 1 } },{bool: {should : [{ match : { bacSi : true } },{ match : { dieuDuong : true } }]}} ],must_not : [{ match : { 'certificateCode.keyword' : '' } }]}}, from : (params.page * 25), size : 25 };
               if (params.term != undefined){
                  var kwords_ = '*' + params.term.toLowerCase() + '*';
                  jsondata = { query: { bool: { must: [ { match: { active: 1 } }, { bool: { should: [ { wildcard : { loginName : kwords_ } }, { match_phrase_prefix : { fullName: { query: kwords_, slop:  3 } } } ] } } ] } }, from : (params.page * 25), size : 25 };
               if(bsdd == 1)
                  jsondata = { query: { bool: { must: [ { match: { active: 1 } },{bool: {should : [{ match : { bacSi : true } },{ match : { dieuDuong : true } }]}}, { bool: { should: [ { wildcard : { loginName : kwords_ } }, { match_phrase_prefix : { fullName: { query: kwords_, slop:  3 } } } ] } } ] } }, from : (params.page * 25), size : 25 };
               else if(bsdd == 2)
                  jsondata = { query: { bool: { must: [ { match: { active: 1 } },{bool: {should : [{ match : { bacSi : true } },{ match : { dieuDuong : true } }]}}, { bool: { should: [ { wildcard : { loginName : kwords_ } }, { match_phrase_prefix : { fullName: { query: kwords_, slop:  3 } } } ] } } ],must_not : [{ match : { 'certificateCode.keyword' : '' } }] } }, from : (params.page * 25), size : 25 };
               }
              var text_ = JSON.stringify(jsondata);
               return text_;
           },
           processResults: function (data, params) {
               params.page = params.page || 0;
               var results = [];
               $.each(data.hits.hits, function (index, item) { results.push({ id: item._id, text: item._source.fullName, Code: item._source.loginName, Name: item._source.fullName, GhiChu: item._source.titleName }); });  
               return { results: results, pagination: { more: (params.page * 10 + data.hits.hits.length) < data.hits.total.value } };
           }
       }, 
       escapeMarkup: function (markup) { return markup; },  
       minimumInputLength: 0, 
       templateResult: formatRepo,  
       templateSelection: formatRepoSelection 
   }); 
}


//mustConditions = [n][3]
//shouldConditions = [n][m][3] hỗ trợ cho dạng biểu thức (a & b) | (c & d)
//searchFields = [n]
//valueField = ""
//showFields = [n][3]: lưu tên trường, số cột hiển thị và trọng số (dùng để css thể hiện mức độ quan trọng) của trường
//selectFields = [n]: lưu tên trường sẽ hiển thị trong cobobox khi được chọn. Nếu null thì mặc định theo showFields
function CallInitSelect2ElAdvanced(Id, svUrl, place, mustConditions, shouldConditions, searchFields, valueField, showFields, selectFields)
{
    var mustQuery = "", shouldQuery = "", searchQuery = "", source = "", processResult = "", templateResult = "", templateSelection = "";
    for(i = 0; i < mustConditions.length; i++)
    {
        subQuery = GetQueryString(mustConditions[i]);
        if(subQuery != null)
        {
            if(mustQuery != "")
                mustQuery += ",";
            mustQuery += subQuery
        }
    }
    for(i = 0; i < shouldConditions.length; i++)
    {
        var subShouldQuery = "";
        for(j = 0; j < shouldConditions[i].length; j++)
        {
            subQuery = GetQueryString(shouldConditions[i][j]);
            if(subQuery != null)
            {
                if(subShouldQuery != "")
                    subShouldQuery += ",";
                subShouldQuery += subQuery;
            }
        }
        if(subShouldQuery != "")
        {
            if(shouldQuery != "")
                shouldQuery += ",";
            shouldQuery+=
                "{ " +
                "     \"bool\": { " +
                "          \"must\": [ " +
                                subShouldQuery +
                "          ] " +
                "     } " +
                "} "
        }
    }
    for(i = 0; i < searchFields.length; i++)
    {
        if(i > 0)
            searchQuery += ",";
        searchQuery+=
            "{ " +
            "    'match_phrase_prefix' : { " +
            "         '" + searchFields[i] + "': { " +
            "             'query': params.term, " +
            "             'slop': 3 " +
            "         } " +
            "    } " +
            "} ";
    }
    var columnNumber = 0;
    for(i = 0; i < showFields.length; i++)
    {
        columnNumber += showFields[i][1];
    }
    for(i = 0; i < showFields.length; i++)
    {
        if(i == 0)
            templateResult = "'<table style=\"width: 100%; margin: -5px; background-color: #E0E9F8;\"><tr>";
        else if(i > 0)
            source += ",";
        source += "\"" + showFields[i][0] + "\"";
        processResult += ", " + showFields[i][0] + ": item._source." + showFields[i][0];
        templateResult += "<td style=\"" + (showFields[i][2]==1? "color:maroon; font-weight:bold;" : "color: black;") + " width:" + (showFields[i][1]*100/columnNumber) + "%;\">' + repo." + showFields[i][0] + " + '</td>";
        if(i == showFields.length - 1)
            templateResult += "</tr></table>'";
    }
    if(selectFields == null)//Lấy theo showFields
    {
        for(i = 0; i < showFields.length; i++)
        {
            if(i > 0)
                templateSelection += " + ' - ' + ";
            templateSelection += "repo." + showFields[i][0];
        }
    }
    else{
        for(i = 0; i < selectFields.length; i++)
        {
            if(i > 0)
                templateSelection += " + ' - ' + ";
            templateSelection += "repo." + selectFields[i];
        }
    }
    var s =
       "$('#' + Id).select2({ " +
        "width: '100%',  " +
        "allowClear: true, " +
        "placeholder:'" + place + "', " +
        "ajax: {  " +
        "    id: function (e) { return e.id },  " + 
        "    url: svUrl,  " +
            //dataType: 'application/json', 
        "    delay: 250,  " +
        "    type: 'POST',   " +
        "    headers: { 'Content-Type': 'application/json' }, " +
            //params:{CodeField: codeField},
        "    data: function (params) { " +              
        "        params.page = params.page || 0; " +
        "        if(params.term == undefined) " +
        "            return JSON.stringify({ " +
        "                \"query\": { " +
        "                    \"bool\": { " +
        "                        \"must\": [ " +
                                   mustQuery + (mustQuery != "" ? "," : null) +
        "                          { " +
        "                              \"bool\": { " +
        "                                  \"should\": [ " +
                                               shouldQuery +
        "                                  ] " +
        "                              } " +
        "                          } " +
        "                        ] " +
        "                    } " +
        "                } " +
        "                ,\"size\": 20 " +
        "                , \"from\": params.page * 20 " +
        "            }); " +
        "        else " + 
        "            return JSON.stringify({ " +
        "                \"query\": { " +
        "                    \"bool\": { " +
        "                        \"must\": [ " +
                                   mustQuery + (mustQuery != "" ? "," : null) +
        "                          { " +
        "                              \"bool\": { " +
        "                                  \"should\": [ " +
                                               shouldQuery +
        "                                  ] " +
        "                              } " +
        "                          }, " +
        "                          { " +
        "                              \"bool\": { " +
        "                                  \"should\": [ " +
                                               searchQuery +
        "                                  ] " +
        "                              } " +
        "                          } " +
        "                        ] " +
        "                    } " +
        "                } " +
        "              ,\"_source\": [" + source + "] " +
        "              ,\"size\": 20 " +
        "              , \"from\": params.page * 20 " +
        "            }); " +
        "    }, " + 
        "    processResults: function (data, params) { " + 
        "        params.page = params.page || 0; " + 
        "        console.log(params.page); " + 
        "        var results = []; " +
        "        $.each(data.hits.hits, function (index, item) { results.push({ id: item._source." + valueField + ", Code: item._source." + valueField + processResult + "}); }); " +
        "        return { " + 
        "            results: results, " + 
        "            pagination: { " + 
        "                more: (params.page * 20 + data.hits.hits.length) < data.hits.total.value" + 
        "            } " + 
        "        }; " + 
        "    }, " + 
        "    cache: true " + 
        "}, " + 
        "escapeMarkup: function (markup) { " + 
        "    return markup; " + 
        "}, " +  
        "minimumInputLength: 0, " + 
        "templateResult: function (repo) { " + 
        "    if (repo.loading) return repo.text; " + 
        "    var markup = " + templateResult + ";" + 
        "    return markup; " + 
        "}, " +  
        "templateSelection: function (repo) { " + 
        "    if(repo.id == '' || repo." + valueField + " == undefined) return repo.text; " + 
        "    return " + templateSelection + "; " + 
        "} " + 
       "}); "; 
    eval(s);
}
//Hàm sinh chuỗi truy vấn elasticsearch
function GetQueryString(input){
    if(input.length != 3)
        return null;
    var field = input[0];
    var operator = input[1];
    var value = input[2];
    var searchString;
    if(operator == operators.term)
    {
        searchString = "{\"term\": {\"" + field + "\": {\"value\": \"" + value + "\"}}}";
    }
    else if(operator == operators.not_term)
    {
        searchString = "{\"bool\": {\"must_not\": {\"term\": {\"" + field + "\": {\"value\": \"" + value + "\"}}}}}";
    }
    else if(operator == operators.match)
    {
        searchString = "{\"match\": {\"" + field + "\": \"" + value + "\"}}";
    }
    else if(operator == operators.not_match)
    {
        searchString = "{\"bool\": {\"must_not\": {\"match\": {\"" + field + "\": \"" + value + "\"}}}}";
    }
    else if(operator == operators.exists)
    {
        searchString = "{\"exists\": {\"field\": \"" + field + "\"}}";
    }
    else if(operator == operators.not_exists)
    {
        searchString = "{\"bool\": {\"must_not\": {\"exists\": {\"field\": \"" + field + "\"}}}}";
    }
    else//Những toán tử còn lại là toán tử phạm vi thì dùng rang
    {
        searchString = "{\"range\": {\"" + field + "\": {\"" + operator + "\": \"" + value + "\"}}}";
    }
    return searchString;
}
//Đây là hàm lấy dữ liệu theo hàm public override HangDoiCls[] PageReadingHangDoi(ActionSqlParamCls ActionSqlParam, HangDoiFilterCls OHangDoiFilter, ref long recordTotal)
//và hàm public override HangDoiCls[] PageReadingHangDoiPTTT_CDHA_CLS(ActionSqlParamCls ActionSqlParam, HangDoiFilterCls OHangDoiFilter, ref long recordTotal) trong danh mục HangDoi
function CallInitSelect2El_HangDoi(Id, svUrl, place, filter) {
    var loaiQuery = '';
    if (filter.Loais != undefined && filter.Loais != null && filter.Loais != '')
    {
        loaiQuery +=
        "                                { " +
        "                                    \"match\": { " +
        "                                        \"loai\": \"|" + filter.Loais[0] + "|\" " +
        "                                    } " +
        "                                } "
        for(i = 1; i < filter.Loais.length; i ++)
        {
            loaiQuery +=
            "                                ,{ " +
            "                                    \"match\": { " +
            "                                        \"loai\": \"|" + filter.Loais[i] + "|\" " +
            "                                    } " +
            "                                } "
        }
        if (loaiQuery != '')
            loaiQuery =
            "                            ,\"should\": [ " +
                                            loaiQuery +
            "                            ] ";

    }
    
    var s =
    "$('#' + Id).select2({ " +
    "    width: '100%', " +
    "    allowClear: true, " +
    "    placeholder: place, " +
    "    ajax: { " +
    "        id: function (e) { return e.id }, " +
    "        url: svUrl, " +
            //dataType: 'application/json',  " +
    "        delay: 250, " +
    "        type: 'POST', " +
    "        headers: { 'Content-Type': 'application/json' }, " +
            //params:{CodeField: codeField}, " +
    "        data: function (params) { " +
    "            params.page = params.page || 0; " +
    "            if (params.term == undefined) " +
    "                return JSON.stringify({ " +
    "                    \"query\": { " +
    "                        \"bool\": { " +
    "                            \"must\": [ " +
    "                                { " +
    "                                   \"term\": { " +
    "                                       \"join_field\": \"hangdoi\" " +
    "                                    } " +
    "                                } " +
    (filter.HieuLuc == undefined || filter.HieuLuc == null ? "" :
    "                                ,{ " +
    "                                    \"term\": { " +
    "                                        \"hieuluc\": " + filter.HieuLuc + " " +
    "                                    } " +
    "                                } ") +
    (filter.KhoaPhong_Id == undefined || filter.KhoaPhong_Id == null || filter.KhoaPhong_Id == '' ? "" :
    "                                ,{ " +
    "                                    \"term\": { " +
    "                                        \"department_id\": \"" + filter.KhoaPhong_Id + "\" " +
    "                                    } " +
    "                                } ") +
    (filter.Departments == undefined || filter.Departments == null || filter.Departments == '' ? "" :
    "                                ,{ " +
    "                                    \"terms\": { " +
    "                                        \"department_id\": " + filter.Departments + " " +
    "                                    } " +
    "                                } ") +
    (filter.Loai == undefined || filter.Loai == null || filter.Loai == '' ? "" :
    "                                ,{ " +
    "                                    \"match\": { " +
    "                                        \"loai\": \"|" + filter.Loai + "|\" " +
    "                                    } " +
    "                                } ") +
    (filter.ChayThongBao == undefined || filter.ChayThongBao == null || filter.ChayThongBao == '' ? "" :
    "                                ,{ " +
    "                                    \"term\": { " +
    "                                        \"chaythongbao\": \"" + filter.ChayThongBao + "\" " +
    "                                    } " +
    "                                } ") +
    (filter.LoginUserId == undefined || filter.LoginUserId == null || filter.LoginUserId == '' ? "" :
    "                                ,{ " +
    "                                   \"has_child\": { " +
    "                                       \"type\": \"user1\", " +
    "                                       \"query\": { " +
    "                                           \"term\": { " +
    "                                               \"owneruser_id\": \"" + filter.LoginUserId + "\" " +
    "                                           } " +
    "                                       } " +
    "                                   } " +
    "                                } ") +
    "                            ] " +
                                loaiQuery +
    "                        } " +
    "                    } " +
    "                    , \"_source\": [\"id\", \"ma\", \"ten\"] " +
    "                    , \"size\": 20 " +
    "                    , \"from\": params.page * 20 " +
    "                    , \"sort\": [ " +
    "                        { " +
    "                            \"ten.keyword\": { " +
    "                                \"order\": \"asc\" " +
    "                            } " +
    "                        } " +
    "                      ] " +
    "                }); " +
    "            else " +
    "                return JSON.stringify({ " +
    "                    \"query\": { " +
    "                        \"bool\": { " +
    "                            \"must\": [ " +
    "                                { " +
    "                                   \"term\": { " +
    "                                       \"join_field\": \"hangdoi\" " +
    "                                    } " +
    "                                } " +
    (filter.HieuLuc == undefined || filter.HieuLuc == null ? "" :
    "                                ,{ " +
    "                                    \"term\": { " +
    "                                        \"hieuluc\": " + filter.HieuLuc + " " +
    "                                    } " +
    "                                } ") +
    (filter.KhoaPhong_Id == undefined || filter.KhoaPhong_Id == null || filter.KhoaPhong_Id == '' ? "" :
    "                                ,{ " +
    "                                    \"term\": { " +
    "                                        \"department_id\": \"" + filter.KhoaPhong_Id + "\" " +
    "                                    } " +
    "                                } ") +
    (filter.Departments == undefined || filter.Departments == null || filter.Departments == '' ? "" :
    "                                ,{ " +
    "                                    \"terms\": { " +
    "                                        \"department_id\": " + filter.Departments + " " +
    "                                    } " +
    "                                } ") +
    (filter.Loai == undefined || filter.Loai == null || filter.Loai == '' ? "" :
    "                                ,{ " +
    "                                    \"match\": { " +
    "                                        \"loai\": \"|" + filter.Loai + "|\" " +
    "                                    } " +
    "                                } ") +
    (filter.ChayThongBao == undefined || filter.ChayThongBao == null || filter.ChayThongBao == '' ? "" :
    "                                ,{ " +
    "                                    \"term\": { " +
    "                                        \"chaythongbao\": \"" + filter.ChayThongBao + "\" " +
    "                                    } " +
    "                                } ") +
    (filter.LoginUserId == undefined || filter.LoginUserId == null || filter.LoginUserId == '' ? "" :
    "                                ,{ " +
    "                                   \"has_child\": { " +
    "                                       \"type\": \"user1\", " +
    "                                       \"query\": { " +
    "                                           \"term\": { " +
    "                                               \"owneruser_id\": \"" + filter.LoginUserId + "\" " +
    "                                           } " +
    "                                       } " +
    "                                   } " +
    "                                } ") +
    "                                ,{ " +
    "                                    \"bool\": { " +
    "                                        \"should\": [ " +
    "                                        { " +
    "                                            \"match_phrase_prefix\": { " +
    "                                                \"ma\": { " +
    "                                                    \"query\": params.term, " +
    "                                                    \"slop\": 3 " +
    "                                                } " +
    "                                            } " +
    "                                        }, " +
    "                                        { " +
    "                                            \"match_phrase_prefix\": { " +
    "                                                \"ten\": { " +
    "                                                    \"query\": params.term, " +
    "                                                    \"slop\": 3 " +
    "                                                } " +
    "                                            } " +
    "                                        }, " +
    "                                        { " +
    "                                            \"match_phrase_prefix\": { " +
    "                                                \"tentat\": { " +
    "                                                    \"query\": params.term, " +
    "                                                    \"slop\": 3 " +
    "                                                } " +
    "                                            } " +
    "                                        }, " +
    "                                        { " +
    "                                            \"wildcard\": { " +
    "                                                \"ma\":  \"*\" + params.term + \"*\"" +
    "                                            } " +
    "                                        }, " +
    "                                        { " +
    "                                            \"wildcard\": { " +
    "                                                \"ten\": \"*\" + params.term + \"*\" " +
    "                                            } " +
    "                                        }, " +
    "                                        { " +
    "                                            \"wildcard\": { " +
    "                                                \"tentat\": \"*\" + params.term + \"*\" " +
    "                                            } " +
    "                                        } " +
    "                                        ] " +
    "                                    } " +
    "                                } " +
    "                            ] " +
                                loaiQuery +
    "                        } " +
    "                    } " +
    "                    , \"_source\": [\"id\", \"ma\", \"ten\"] " +
    "                    , \"size\": 20 " +
    "                    , \"from\": params.page * 20 " +
    "                    , \"sort\": [ " +
    "                        { " +
    "                            \"ten.keyword\": { " +
    "                                \"order\": \"asc\" " +
    "                            } " +
    "                        } " +
    "                      ] " +
    "                }); " +
    "        }, " +
    "        processResults: function (data, params) { " +
    "            params.page = params.page || 0; " +
    "            console.log(params.page); " +
    "            var results = []; " +
    "            $.each(data.hits.hits, function (index, item) { results.push({ id: item._source.id, ma: item._source.ma, ten: item._source.ten, text: item._source.ten }); }); " +
    "            return { " +
    "                results: results, " +
    "                pagination: { " +
    "                    more: (params.page * 20 + data.hits.hits.length) < data.hits.total.value " +
    "                } " +
    "            }; " +
    "        }, " +
    "        cache: true " +
    "    }, " +
    "    escapeMarkup: function (markup) { " +
    "        return markup; " +
    "    }, " +
    "    minimumInputLength: 0, " +
    "    templateResult: function (repo) { " +
    "        var markup = '';  " +
    "        if (repo.loading) return repo.text;  " +
    "        else if (repo.id == null) markup = '<table style=\"width: 100%;border-bottom: 1px solid black;\"><tr><td style=\"width:20%;padding:4px; text-align: left;\"><h3>'+ repo.ma+'</h3></td> <td style=\"text-align: left;\"><h3>'+repo.ten+'</h3></td></tr></table>';  " +
    "        else markup = '<table style=\"width: 100%;\"><tr><td style=\"color:maroon;font-weight:bold; width:20%;padding:4px; text-align: left;\">'+ repo.ma+'</td> <td style=\"text-align: left;\">'+repo.ten+'</td></tr></table>';  " +
    "        return markup;  " +
    "    }, " +
    "    templateSelection: function (repo) { " +
    "        if (repo.ma == undefined) return repo.text; " +
    "        return repo.text + \"(\" + repo.ma + \")\"; " +
    "    } " +
    "}); ";
    eval(s);
}

//Đây là hàm lấy dữ liệu theo hàm public override HangDoiCls[] PageReading(ActionSqlParamCls ActionSqlParam, HangDoiFilterCls OHangDoiFilter, ref long recordTotal) trong danh mục HangDoi
function CallInitSelect2ElClass_HangDoi(Id, svUrl, place, filter) {
    var loaiQuery = '';
    if (filter.Loais != undefined && filter.Loais != null && filter.Loais != '') {
        loaiQuery +=
        "                                { " +
        "                                    \"match\": { " +
        "                                        \"loai\": \"|" + filter.Loais[0] + "|\" " +
        "                                    } " +
        "                                } "
        for (i = 0; i < filter.Loais.length; i++) {
            loaiQuery +=
            "                                ,{ " +
            "                                    \"match\": { " +
            "                                        \"loai\": \"|" + filter.Loais[i] + "|\" " +
            "                                    } " +
            "                                } "
        }
        if (loaiQuery != '')
            loaiQuery =
            "                            ,\"should\": [ " +
                                            loaiQuery +
            "                            ] ";

    }
    var s =
    "$(\".\" + Id).select2({ " +
    "    width: '100%', " +
    "    allowClear: true, " +
    "    placeholder: place, " +
    "    ajax: { " +
    "        id: function (e) { return e.id }, " +
    "        url: svUrl, " +
            //dataType: 'application/json',  " +
    "        delay: 250, " +
    "        type: 'POST', " +
    "        headers: { 'Content-Type': 'application/json' }, " +
            //params:{CodeField: codeField}, " +
    "        data: function (params) { " +
    "            params.page = params.page || 0; " +
    "            if (params.term == undefined) " +
    "                return JSON.stringify({ " +
    "                    \"query\": { " +
    "                        \"bool\": { " +
    "                            \"must\": [ " +
    "                                { " +
    "                                   \"term\": { " +
    "                                       \"join_field\": \"hangdoi\" " +
    "                                    } " +
    "                                }, " +
    "                                { " +
    "                                    \"has_child\": { " +
    "                                        \"type\": \"dichvu\", " +
    "                                        \"query\": { " +
    "                                            \"bool\": { " +
    "                                                \"must\": [ " +
    "                                                { " +
    "                                                   \"match_all\": {} " +
    "                                                } " +
    (filter.HieuLuc == undefined || filter.HieuLuc == '' || filter.HieuLuc == null ? "" :
    "                                                ,{ " +
    "                                                   \"term\": { " +
    "                                                       \"dichvu_hieuluc\": { " +
    "                                                           \"value\": " + filter.HieuLuc + " " +
    "                                                       } " +
    "                                                   } " +
    "                                               } ") +
    (filter.LopDichVu == undefined || filter.LopDichVu == null || filter.LopDichVu == '' ? "" :
    "                                               ,{ " +
    "                                                   \"term\": { " +
    "                                                       \"lopdichvu\": { " +
    "                                                           \"value\": " + filter.LopDichVu + " " +
    "                                                       } " +
    "                                                   } " +
    "                                               } ") +
    (filter.DichVu_ID == undefined || filter.DichVu_ID == '' || filter.DichVu_ID == null ? "" :
    "                                               ,{ " +
    "                                                   \"term\": { " +
    "                                                       \"dichvu_id\": \"" + filter.DichVu_ID + "\" " +
    "                                                   } " +
    "                                               } ") +
    "                                               ] " +
    "                                            } " +
    "                                        } " +
    "                                    } " +
    "                                } " +
    (filter.HieuLuc == undefined || filter.HieuLuc == '' || filter.HieuLuc == null ? "" :
    "                                ,{ " +
    "                                    \"term\": { " +
    "                                        \"hieuluc\": " + filter.HieuLuc + " " +
    "                                    } " +
    "                                } ") +
    (filter.DepartmentId == undefined || filter.DepartmentId == '' || filter.DepartmentId == null ? "" :
    "                                ,{ " +
    "                                    \"term\": { " +
    "                                        \"department_id\": \"" + filter.DepartmentId + "\" " +
    "                                    } " +
    "                                } ") +
    (filter.HangDoi_ID == undefined || filter.HangDoi_ID == '' || filter.HangDoi_ID == null ? "" :
    "                                ,{ " +
    "                                    \"term\": { " +
    "                                        \"id\": \"" + filter.HangDoi_ID + "\" " +
    "                                    } " +
    "                                } ") +
    (filter.Loai == undefined || filter.Loai == null || filter.Loai == '' ? "" :
    "                                ,{ " +
    "                                    \"match\": { " +
    "                                        \"loai\": \"|" + filter.Loai + "|\" " +
    "                                    } " +
    "                                } ") +
    (filter.LoginUserId == undefined || filter.LoginUserId == null || filter.LoginUserId == '' ? "" :
    "                                ,{ " +
    "                                   \"has_child\": { " +
    "                                       \"type\": \"user1\", " +
    "                                       \"query\": { " +
    "                                           \"term\": { " +
    "                                               \"owneruser_id\": \"" + filter.LoginUserId + "\" " +
    "                                           } " +
    "                                       } " +
    "                                   } " +
    "                                } ") +
    "                            ] " +
                                loaiQuery +
    (filter.NotInLoai == undefined || filter.NotInLoai == null || filter.NotInLoai == '' ? "" :
    "                            ,\"must_not\": [ " +
    "                                { " +
    "                                    \"match\": { " +
    "                                        \"loai\": \"|" + filter.NotInLoai + "|\" " +
    "                                    } " +
    "                                } " +
    "                            ] ") +
    "                        } " +
    "                    } " +
    "                    , \"_source\": [\"id\", \"ma\", \"ten\"] " +
    "                    , \"size\": 20 " +
    "                    , \"from\": params.page * 20 " +
    "                    , \"sort\": [ " +
    "                        { " +
    "                            \"ten.keyword\": { " +
    "                                \"order\": \"asc\" " +
    "                            } " +
    "                        } " +
    "                      ] " +
    "                }); " +
    "            else " +
    "                return JSON.stringify({ " +
    "                    \"query\": { " +
    "                        \"bool\": { " +
    "                            \"must\": [ " +
    "                                { " +
    "                                   \"term\": { " +
    "                                       \"join_field\": \"hangdoi\" " +
    "                                    } " +
    "                                }, " +
    "                                { " +
    "                                    \"bool\": { " +
    "                                        \"should\": [ " +
    "                                        { " +
    "                                            \"match_phrase_prefix\": { " +
    "                                                \"ma\": { " +
    "                                                    \"query\": params.term, " +
    "                                                    \"slop\": 3 " +
    "                                                } " +
    "                                            } " +
    "                                        }, " +
    "                                        { " +
    "                                            \"match_phrase_prefix\": { " +
    "                                                \"ten\": { " +
    "                                                    \"query\": params.term, " +
    "                                                    \"slop\": 3 " +
    "                                                } " +
    "                                            } " +
    "                                        }, " +
    "                                        { " +
    "                                            \"match_phrase_prefix\": { " +
    "                                                \"tentat\": { " +
    "                                                    \"query\": params.term, " +
    "                                                    \"slop\": 3 " +
    "                                                } " +
    "                                            } " +
    "                                        }, " +
    "                                        { " +
    "                                            \"wildcard\": { " +
    "                                                \"ma\":  \"*\" + params.term + \"*\"" +
    "                                            } " +
    "                                        }, " +
    "                                        { " +
    "                                            \"wildcard\": { " +
    "                                                \"ten\": \"*\" + params.term + \"*\" " +
    "                                            } " +
    "                                        }, " +
    "                                        { " +
    "                                            \"wildcard\": { " +
    "                                                \"tentat\": \"*\" + params.term + \"*\" " +
    "                                            } " +
    "                                        } " +
    "                                        ] " +
    "                                    } " +
    "                                }, " +
    "                                { " +
    "                                    \"has_child\": { " +
    "                                        \"type\": \"dichvu\", " +
    "                                        \"query\": { " +
    "                                            \"bool\": { " +
    "                                                \"must\": [ " +
    "                                                { " +
    "                                                   \"match_all\": {} " +
    "                                                } " +
    (filter.HieuLuc == undefined || filter.HieuLuc == '' || filter.HieuLuc == null ? "" :
    "                                                ,{ " +
    "                                                   \"term\": { " +
    "                                                       \"dichvu_hieuluc\": { " +
    "                                                           \"value\": " + filter.HieuLuc + " " +
    "                                                       } " +
    "                                                   } " +
    "                                               } ") +
    (filter.LopDichVu == undefined || filter.LopDichVu == null || filter.LopDichVu == '' ? "" :
    "                                               ,{ " +
    "                                                   \"term\": { " +
    "                                                       \"lopdichvu\": { " +
    "                                                           \"value\": " + filter.LopDichVu + " " +
    "                                                       } " +
    "                                                   } " +
    "                                               } ") +
    (filter.DichVu_ID == undefined || filter.DichVu_ID == '' || filter.DichVu_ID == null ? "" :
    "                                               ,{ " +
    "                                                   \"term\": { " +
    "                                                       \"dichvu_id\": \"" + filter.DichVu_ID + "\" " +
    "                                                   } " +
    "                                               } ") +
    "                                               ] " +
    "                                            } " +
    "                                        } " +
    "                                    } " +
    "                                } " +
    (filter.HieuLuc == undefined || filter.HieuLuc == '' || filter.HieuLuc == null ? "" :
    "                                ,{ " +
    "                                    \"term\": { " +
    "                                        \"hieuluc\": " + filter.HieuLuc + " " +
    "                                    } " +
    "                                } ") +
    (filter.DepartmentId == undefined || filter.DepartmentId == '' || filter.DepartmentId == null ? "" :
    "                                ,{ " +
    "                                    \"term\": { " +
    "                                        \"department_id\": \"" + filter.DepartmentId + "\" " +
    "                                    } " +
    "                                } ") +
    (filter.HangDoi_ID == undefined || filter.HangDoi_ID == '' || filter.HangDoi_ID == null ? "" :
    "                                ,{ " +
    "                                    \"term\": { " +
    "                                        \"id\": \"" + filter.HangDoi_ID + "\" " +
    "                                    } " +
    "                                } ") +
    (filter.Loai == undefined || filter.Loai == null || filter.Loai == '' ? "" :
    "                                ,{ " +
    "                                    \"match\": { " +
    "                                        \"loai\": \"|" + filter.Loai + "|\" " +
    "                                    } " +
    "                                } ") +
    (filter.LoginUserId == undefined || filter.LoginUserId == null || filter.LoginUserId == '' ? "" :
    "                                ,{ " +
    "                                   \"has_child\": { " +
    "                                       \"type\": \"user1\", " +
    "                                       \"query\": { " +
    "                                           \"term\": { " +
    "                                               \"owneruser_id\": \"" + filter.LoginUserId + "\" " +
    "                                           } " +
    "                                       } " +
    "                                   } " +
    "                                } ") +
    "                            ] " +
                                loaiQuery +
    (filter.NotInLoai == undefined || filter.NotInLoai == null || filter.NotInLoai == '' ? "" :
    "                            ,\"must_not\": [ " +
    "                                { " +
    "                                    \"match\": { " +
    "                                        \"loai\": \"|" + filter.NotInLoai + "|\" " +
    "                                    } " +
    "                                } " +
    "                            ] ") +
    "                        } " +
    "                    } " +
    "                    , \"_source\": [\"id\", \"ma\", \"ten\"] " +
    "                    , \"size\": 20 " +
    "                    , \"from\": params.page * 20 " +
    "                    , \"sort\": [ " +
    "                        { " +
    "                            \"ten.keyword\": { " +
    "                                \"order\": \"asc\" " +
    "                            } " +
    "                        } " +
    "                      ] " +
    "                }); " +
    "        }, " +
    "        processResults: function (data, params) { " +
    "            params.page = params.page || 0; " +
    "            console.log(params.page); " +
    "            var results = []; " +
    "            $.each(data.hits.hits, function (index, item) { results.push({ id: item._source.id, ma: item._source.ma, ten: item._source.ten, text: item._source.ten }); }); " +
    "            return { " +
    "                results: results, " +
    "                pagination: { " +
    "                    more: (params.page * 20 + data.hits.hits.length) < data.hits.total.value " +
    "                } " +
    "            }; " +
    "        }, " +
    "        cache: true " +
    "    }, " +
    "    escapeMarkup: function (markup) { " +
    "        return markup; " +
    "    }, " +
    "    minimumInputLength: 0, " +
    "    templateResult: function (repo) { " +
    "        var markup = '';  " +
    "        if (repo.loading) return repo.text;  " +
    "        else if (repo.id == null) markup = '<table style=\"width: 100%;border-bottom: 1px solid black;\"><tr><td style=\"width:20%;padding:4px; text-align: left;\"><h3>'+ repo.ma+'</h3></td> <td style=\"text-align: left;\"><h3>'+repo.ten+'</h3></td></tr></table>';  " +
    "        else markup = '<table style=\"width: 100%;\"><tr><td style=\"color:maroon;font-weight:bold; width:20%;padding:4px; text-align: left;\">'+ repo.ma+'</td> <td style=\"text-align: left;\">'+repo.ten+'</td></tr></table>';  " +
    "        return markup;  " +
    "    }, " +
    "    templateSelection: function (repo) { " +
    "        if (repo.ma == undefined) return repo.text; " +
    "        return repo.text + \"(\" + repo.ma + \")\"; " +
    "    } " +
    "}); ";
    eval(s);
}


//Đây là hàm lấy dữ liệu theo hàm public override HangDoiCls[] GetByCD(ActionSqlParamCls ActionSqlParam, DichVuByParentIdFilterCls ODichVuFilter) trong danh mục HangDoi
function CallInitSelect2El_HangDoiDichVuCD(Id, svUrl, place, filter) {
    var s =
    "$('#' + Id).select2({ " +
    "    width: '100%', " +
    "    allowClear: true, " +
    "    placeholder: place, " +
    "    ajax: { " +
    "        id: function (e) { return e.id }, " +
    "        url: svUrl, " +
            //dataType: 'application/json',  " +
    "        delay: 250, " +
    "        type: 'POST', " +
    "        headers: { 'Content-Type': 'application/json' }, " +
            //params:{CodeField: codeField}, " +
    "        data: function (params) { " +
    "            params.page = params.page || 0; " +
    "            if (params.term == undefined) " +
    "                return JSON.stringify({ " +
    "                    \"query\": { " +
    "                        \"bool\": { " +
    "                            \"must\": [ " +
    "                                { " +
    "                                   \"term\": { " +
    "                                       \"join_field\": \"hangdoi\" " +
    "                                    } " +
    "                                }, " +
    "                                { " +
    "                                    \"term\": { " +
    "                                        \"hieuluc\": 1 " +
    "                                    } " +
    "                                }, " +
    (filter.HangDoi_ID == undefined || filter.HangDoi_ID == '' || filter.HangDoi_ID == null ? "" :
    "                                { " +
    "                                    \"term\": { " +
    "                                        \"id\": \"" + filter.HangDoi_ID + "\" " +
    "                                    } " +
    "                                }, ") +
    (filter.IsChuyenYeuCau == undefined || filter.IsChuyenYeuCau == '' || filter.IsChuyenYeuCau == null || filter.IsChuyenYeuCau == 0 ? "" :
    "                                { " +
    "                                    \"term\": { " +
    "                                        \"chuyenyeucau\": \"" + filter.IsChuyenYeuCau + "\" " +
    "                                    } " +
    "                                }, ") +
    (filter.LoginUserId == undefined || filter.LoginUserId == null || filter.LoginUserId == '' ? "" :
    "                                { " +
    "                                   \"has_child\": { " +
    "                                       \"type\": \"user1\", " +
    "                                       \"query\": { " +
    "                                           \"term\": { " +
    "                                               \"owneruser_id\": \"" + filter.LoginUserId + "\" " +
    "                                           } " +
    "                                       } " +
    "                                   } " +
    "                                }, ") +
    "                                { " +
    "                                    \"has_child\": { " +
    "                                        \"type\": \"dichvu\", " +
    "                                        \"query\": { " +
    "                                            \"bool\": { " +
    "                                                \"must\": [ " +
    "                                                { " +
    "                                                   \"term\": { " +
    "                                                       \"dichvu_hieuluc\": { " +
    "                                                           \"value\": 1 " +
    "                                                       } " +
    "                                                   } " +
    "                                               }, " +
    (filter.LopDichVu == undefined || filter.LopDichVu == null || filter.LopDichVu == '' ? "" :
    "                                               { " +
    "                                                   \"term\": { " +
    "                                                       \"lopdichvu\": { " +
    "                                                           \"value\": " + filter.LopDichVu + " " +
    "                                                       } " +
    "                                                   } " +
    "                                               }, ") +
    "                                               { " +
    "                                                   \"bool\": { " +
    "                                                       \"should\": [ " +
    "                                                       { " +
    "                                                           \"term\": { " +
    "                                                               \"loaidichvu_id\": { " +
    "                                                                   \"value\": \"" + filter.ParentId + "\" " +
    "                                                               } " +
    "                                                           } " +
    "                                                       }, " +
    "                                                       { " +
    "                                                           \"term\": { " +
    "                                                               \"nhomdichvu_id\": { " +
    "                                                                   \"value\": \"" + filter.ParentId + "\" " +
    "                                                               } " +
    "                                                           } " +
    "                                                       } " +
    "                                                       ] " +
    "                                                   } " +
    "                                               } " +
    "                                               ] " +
    "                                            } " +
    "                                        } " +
    "                                    } " +
    "                                } " +
    "                            ] " +
    "                        } " +
    "                    } " +
    "                    , \"_source\": [\"id\", \"ma\", \"ten\"] " +
    "                    , \"size\": 20 " +
    "                    , \"from\": params.page * 20 " +
    "                    , \"sort\": [ " +
    "                        { " +
    "                            \"ten.keyword\": { " +
    "                                \"order\": \"asc\" " +
    "                            } " +
    "                        } " +
    "                      ] " +
    "                }); " +
    "            else " +
    "                return JSON.stringify({ " +
    "                    \"query\": { " +
    "                        \"bool\": { " +
    "                            \"must\": [ " +
    "                                { " +
    "                                   \"term\": { " +
    "                                       \"join_field\": \"hangdoi\" " +
    "                                    } " +
    "                                }, " +
    "                                { " +
    "                                    \"term\": { " +
    "                                        \"hieuluc\": 1 " +
    "                                    } " +
    "                                }, " +
    (filter.HangDoi_ID == undefined || filter.HangDoi_ID == '' || filter.HangDoi_ID == null ? "" :
    "                                { " +
    "                                    \"term\": { " +
    "                                        \"id\": \"" + filter.HangDoi_ID + "\" " +
    "                                    } " +
    "                                }, ") +
    (filter.IsChuyenYeuCau == undefined || filter.IsChuyenYeuCau == '' || filter.IsChuyenYeuCau == null || filter.IsChuyenYeuCau == 0 ? "" :
    "                                { " +
    "                                    \"term\": { " +
    "                                        \"chuyenyeucau\": \"" + filter.IsChuyenYeuCau + "\" " +
    "                                    } " +
    "                                }, ") +
    (filter.LoginUserId == undefined || filter.LoginUserId == null || filter.LoginUserId == '' ? "" :
    "                                { " +
    "                                   \"has_child\": { " +
    "                                       \"type\": \"user1\", " +
    "                                       \"query\": { " +
    "                                           \"term\": { " +
    "                                               \"owneruser_id\": \"" + filter.LoginUserId + "\" " +
    "                                           } " +
    "                                       } " +
    "                                   } " +
    "                                }, ") +
    "                                { " +
    "                                    \"bool\": { " +
    "                                        \"should\": [ " +
    "                                        { " +
    "                                            \"match_phrase_prefix\": { " +
    "                                                \"ma\": { " +
    "                                                    \"query\": params.term, " +
    "                                                    \"slop\": 3 " +
    "                                                } " +
    "                                            } " +
    "                                        }, " +
    "                                        { " +
    "                                            \"match_phrase_prefix\": { " +
    "                                                \"ten\": { " +
    "                                                    \"query\": params.term, " +
    "                                                    \"slop\": 3 " +
    "                                                } " +
    "                                            } " +
    "                                        }, " +
    "                                        { " +
    "                                            \"match_phrase_prefix\": { " +
    "                                                \"tentat\": { " +
    "                                                    \"query\": params.term, " +
    "                                                    \"slop\": 3 " +
    "                                                } " +
    "                                            } " +
    "                                        }, " +
    "                                        { " +
    "                                            \"wildcard\": { " +
    "                                                \"ma\":  \"*\" + params.term + \"*\"" +
    "                                            } " +
    "                                        }, " +
    "                                        { " +
    "                                            \"wildcard\": { " +
    "                                                \"ten\": \"*\" + params.term + \"*\" " +
    "                                            } " +
    "                                        }, " +
    "                                        { " +
    "                                            \"wildcard\": { " +
    "                                                \"tentat\": \"*\" + params.term + \"*\" " +
    "                                            } " +
    "                                        } " +
    "                                        ] " +
    "                                    } " +
    "                                }, " +
    "                                { " +
    "                                    \"has_child\": { " +
    "                                        \"type\": \"dichvu\", " +
    "                                        \"query\": { " +
    "                                            \"bool\": { " +
    "                                                \"must\": [ " +
    "                                                { " +
    "                                                   \"term\": { " +
    "                                                       \"dichvu_hieuluc\": { " +
    "                                                           \"value\": 1 " +
    "                                                       } " +
    "                                                   } " +
    "                                               }, " +
    (filter.LopDichVu == undefined || filter.LopDichVu == null || filter.LopDichVu == '' ? "" :
    "                                               { " +
    "                                                   \"term\": { " +
    "                                                       \"lopdichvu\": { " +
    "                                                           \"value\": " + filter.LopDichVu + " " +
    "                                                       } " +
    "                                                   } " +
    "                                               }, ") +
    "                                               { " +
    "                                                   \"bool\": { " +
    "                                                       \"should\": [ " +
    "                                                       { " +
    "                                                           \"term\": { " +
    "                                                               \"loaidichvu_id\": { " +
    "                                                                   \"value\": \"" + filter.ParentId + "\" " +
    "                                                               } " +
    "                                                           } " +
    "                                                       }, " +
    "                                                       { " +
    "                                                           \"term\": { " +
    "                                                               \"nhomdichvu_id\": { " +
    "                                                                   \"value\": \"" + filter.ParentId + "\" " +
    "                                                               } " +
    "                                                           } " +
    "                                                       } " +
    "                                                       ] " +
    "                                                   } " +
    "                                               } " +
    "                                               ] " +
    "                                            } " +
    "                                        } " +
    "                                    } " +
    "                                } " +
    "                            ] " +
    "                        } " +
    "                    } " +
    "                    , \"_source\": [\"id\", \"ma\", \"ten\"] " +
    "                    , \"size\": 20 " +
    "                    , \"from\": params.page * 20 " +
    "                    , \"sort\": [ " +
    "                        { " +
    "                            \"ten.keyword\": { " +
    "                                \"order\": \"asc\" " +
    "                            } " +
    "                        } " +
    "                      ] " +
    "                }); " +
    "        }, " +
    "        processResults: function (data, params) { " +
    "            params.page = params.page || 0; " +
    "            console.log(params.page); " +
    "            var results = []; " +
    "            $.each(data.hits.hits, function (index, item) { results.push({ id: item._source.id, ma: item._source.ma, ten: item._source.ten, text: item._source.ten }); }); " +
    "            return { " +
    "                results: results, " +
    "                pagination: { " +
    "                    more: (params.page * 20 + data.hits.hits.length) < data.hits.total.value " +
    "                } " +
    "            }; " +
    "        }, " +
    "        cache: true " +
    "    }, " +
    "    escapeMarkup: function (markup) { " +
    "        return markup; " +
    "    }, " +
    "    minimumInputLength: 0, " +
    "    templateResult: function (repo) { " +
    "        var markup = '';  " +
    "        if (repo.loading) return repo.text;  " +
    "        else if (repo.id == null) markup = '<table style=\"width: 100%;border-bottom: 1px solid black;\"><tr><td style=\"width:20%;padding:4px;text-align: left;\"><h3>'+ repo.ma+'</h3></td> <td style=\"text-align: left;\"><h3>'+repo.ten+'</h3></td></tr></table>';  " +
    "        else markup = '<table style=\"width: 100%;\"><tr><td style=\"color:maroon;font-weight:bold; width:20%;padding:4px;text-align: left;\">'+ repo.ma+'</td> <td style=\"text-align: left;\">'+repo.ten+'</td></tr></table>';  " +
    "        return markup;  " +
    "    }, " +
    "    templateSelection: function (repo) { " +
    "        if (repo.ma == undefined) return repo.text; " +
    "        return repo.text + \"(\" + repo.ma + \")\"; " +
    "    } " +
    "}); ";
    eval(s);
}

//Đây là hàm lấy dữ liệu theo hàm public override HangDoiCls[] PageReading(ActionSqlParamCls ActionSqlParam, HangDoiFilterCls OHangDoiFilter, ref long recordTotal) trong danh mục HangDoi
function CallInitSelect2El_HangDoiDichVu(Id, svUrl, place, filter) {
    var loaiQuery = '';
    if (filter.Loais != undefined && filter.Loais != null && filter.Loais != '')
    {
        loaiQuery +=
        "                                { " +
        "                                    \"match\": { " +
        "                                        \"loai\": \"|" + filter.Loais[0] + "|\" " +
        "                                    } " +
        "                                } "
        for(i = 0; i < filter.Loais.length; i ++)
        {
            loaiQuery +=
            "                                ,{ " +
            "                                    \"match\": { " +
            "                                        \"loai\": \"|" + filter.Loais[i] + "|\" " +
            "                                    } " +
            "                                } "
        }
        if (loaiQuery != '')
            loaiQuery =
            "                            ,\"should\": [ " +
                                            loaiQuery +
            "                            ] ";

    }
    var s =
    "$('#' + Id).select2({ " +
    "    width: '100%', " +
    "    allowClear: true, " +
    "    placeholder: place, " +
    "    ajax: { " +
    "        id: function (e) { return e.id }, " +
    "        url: svUrl, " +
            //dataType: 'application/json',  " +
    "        delay: 250, " +
    "        type: 'POST', " +
    "        headers: { 'Content-Type': 'application/json' }, " +
            //params:{CodeField: codeField}, " +
    "        data: function (params) { " +
    "            params.page = params.page || 0; " +
    "            if (params.term == undefined) " +
    "                return JSON.stringify({ " +
    "                    \"query\": { " +
    "                        \"bool\": { " +
    "                            \"must\": [ " +
    "                                { " +
    "                                   \"term\": { " +
    "                                       \"join_field\": \"hangdoi\" " +
    "                                    } " +
    "                                }, " +
    "                                { " +
    "                                    \"has_child\": { " +
    "                                        \"type\": \"dichvu\", " +
    "                                        \"query\": { " +
    "                                            \"bool\": { " +
    "                                                \"must\": [ " +
    "                                                { " +
    "                                                   \"match_all\": {} " +
    "                                                } " +
    (filter.HieuLuc == undefined || filter.HieuLuc == '' || filter.HieuLuc == null ? "" :
    "                                                ,{ " +
    "                                                   \"term\": { " +
    "                                                       \"dichvu_hieuluc\": { " +
    "                                                           \"value\": " + filter.HieuLuc + " " +
    "                                                       } " +
    "                                                   } " +
    "                                               } ") +
    (filter.LopDichVu == undefined || filter.LopDichVu == null || filter.LopDichVu == '' ? "" :
    "                                               ,{ " +
    "                                                   \"term\": { " +
    "                                                       \"lopdichvu\": { " +
    "                                                           \"value\": " + filter.LopDichVu + " " +
    "                                                       } " +
    "                                                   } " +
    "                                               } ") +
    (filter.DichVu_ID == undefined || filter.DichVu_ID == '' || filter.DichVu_ID == null ? "" :
    "                                               ,{ " +
    "                                                   \"term\": { " +
    "                                                       \"dichvu_id\": \"" + filter.DichVu_ID + "\" " +
    "                                                   } " +
    "                                               } ") +
    "                                               ] " +
    "                                            } " +
    "                                        } " +
    "                                    } " +
    "                                } " +
    (filter.HieuLuc == undefined || filter.HieuLuc == '' || filter.HieuLuc == null ? "" :
    "                                ,{ " +
    "                                    \"term\": { " +
    "                                        \"hieuluc\": " + filter.HieuLuc + " " +
    "                                    } " +
    "                                } ") +
    (filter.DepartmentId == undefined || filter.DepartmentId == '' || filter.DepartmentId == null ? "" :
    "                                ,{ " +
    "                                    \"term\": { " +
    "                                        \"department_id\": \"" + filter.DepartmentId + "\" " +
    "                                    } " +
    "                                } ") +
    (filter.HangDoi_ID == undefined || filter.HangDoi_ID == '' || filter.HangDoi_ID == null ? "" :
    "                                ,{ " +
    "                                    \"term\": { " +
    "                                        \"id\": \"" + filter.HangDoi_ID + "\" " +
    "                                    } " +
    "                                } ") +
    (filter.Loai == undefined || filter.Loai == null || filter.Loai == '' ? "" :
    "                                ,{ " +
    "                                    \"match\": { " +
    "                                        \"loai\": \"|" + filter.Loai + "|\" " +
    "                                    } " +
    "                                } ") +
    (filter.LoginUserId == undefined || filter.LoginUserId == null || filter.LoginUserId == '' ? "" :
    "                                ,{ " +
    "                                   \"has_child\": { " +
    "                                       \"type\": \"user1\", " +
    "                                       \"query\": { " +
    "                                           \"term\": { " +
    "                                               \"owneruser_id\": \"" + filter.LoginUserId + "\" " +
    "                                           } " +
    "                                       } " +
    "                                   } " +
    "                                } ") +
    "                            ] " +
                                loaiQuery +
    "                            ,\"must_not\": [ " +
    "                                { " +
    "                                    \"match\": { " +
    "                                        \"\": \"\" " +
    "                                    } " +
    "                                } " +
    (filter.NotInLoai == undefined || filter.NotInLoai == null || filter.NotInLoai == '' ? "" :
    "                                ,{ " +
    "                                    \"match\": { " +
    "                                        \"loai\": \"|" + filter.NotInLoai + "|\" " +
    "                                    } " +
    "                                } ") +
    (filter.NotInHangDoi_ID == undefined || filter.NotInHangDoi_ID == null || filter.NotInHangDoi_ID == '' ? "" :
    "                                ,{ " +
    "                                    \"match\": { " +
    "                                        \"id\": \"" + filter.NotInHangDoi_ID + "\" " +
    "                                    } " +
    "                                } ") +
    "                            ] " +
    "                        } " +
    "                    } " +
    "                    , \"_source\": [\"id\", \"ma\", \"ten\"] " +
    "                    , \"size\": 20 " +
    "                    , \"from\": params.page * 20 " +
    "                    , \"sort\": [ " +
    "                        { " +
    "                            \"ten.keyword\": { " +
    "                                \"order\": \"asc\" " +
    "                            } " +
    "                        } " +
    "                      ] " +
    "                }); " +
    "            else " +
    "                return JSON.stringify({ " +
    "                    \"query\": { " +
    "                        \"bool\": { " +
    "                            \"must\": [ " +
    "                                { " +
    "                                   \"term\": { " +
    "                                       \"join_field\": \"hangdoi\" " +
    "                                    } " +
    "                                }, " +
    "                                { " +
    "                                    \"bool\": { " +
    "                                        \"should\": [ " +
    "                                        { " +
    "                                            \"match_phrase_prefix\": { " +
    "                                                \"ma\": { " +
    "                                                    \"query\": params.term, " +
    "                                                    \"slop\": 3 " +
    "                                                } " +
    "                                            } " +
    "                                        }, " +
    "                                        { " +
    "                                            \"match_phrase_prefix\": { " +
    "                                                \"ten\": { " +
    "                                                    \"query\": params.term, " +
    "                                                    \"slop\": 3 " +
    "                                                } " +
    "                                            } " +
    "                                        }, " +
    "                                        { " +
    "                                            \"match_phrase_prefix\": { " +
    "                                                \"tentat\": { " +
    "                                                    \"query\": params.term, " +
    "                                                    \"slop\": 3 " +
    "                                                } " +
    "                                            } " +
    "                                        }, " +
    "                                        { " +
    "                                            \"wildcard\": { " +
    "                                                \"ma\":  \"*\" + params.term + \"*\"" +
    "                                            } " +
    "                                        }, " +
    "                                        { " +
    "                                            \"wildcard\": { " +
    "                                                \"ten\": \"*\" + params.term + \"*\" " +
    "                                            } " +
    "                                        }, " +
    "                                        { " +
    "                                            \"wildcard\": { " +
    "                                                \"tentat\": \"*\" + params.term + \"*\" " +
    "                                            } " +
    "                                        } " +
    "                                        ] " +
    "                                    } " +
    "                                }, " +
    "                                { " +
    "                                    \"has_child\": { " +
    "                                        \"type\": \"dichvu\", " +
    "                                        \"query\": { " +
    "                                            \"bool\": { " +
    "                                                \"must\": [ " +
    "                                                { " +
    "                                                   \"match_all\": {} " +
    "                                                } " +
    (filter.HieuLuc == undefined || filter.HieuLuc == '' || filter.HieuLuc == null ? "" :
    "                                                ,{ " +
    "                                                   \"term\": { " +
    "                                                       \"dichvu_hieuluc\": { " +
    "                                                           \"value\": " + filter.HieuLuc + " " +
    "                                                       } " +
    "                                                   } " +
    "                                               } ") +
    (filter.LopDichVu == undefined || filter.LopDichVu == null || filter.LopDichVu == '' ? "" :
    "                                               ,{ " +
    "                                                   \"term\": { " +
    "                                                       \"lopdichvu\": { " +
    "                                                           \"value\": " + filter.LopDichVu + " " +
    "                                                       } " +
    "                                                   } " +
    "                                               } ") +
    (filter.DichVu_ID == undefined || filter.DichVu_ID == '' || filter.DichVu_ID == null ? "" :
    "                                               ,{ " +
    "                                                   \"term\": { " +
    "                                                       \"dichvu_id\": \"" + filter.DichVu_ID + "\" " +
    "                                                   } " +
    "                                               } ") +
    "                                               ] " +
    "                                            } " +
    "                                        } " +
    "                                    } " +
    "                                } " +
    (filter.HieuLuc == undefined || filter.HieuLuc == '' || filter.HieuLuc == null ? "" :
    "                                ,{ " +
    "                                    \"term\": { " +
    "                                        \"hieuluc\": " + filter.HieuLuc + " " +
    "                                    } " +
    "                                } ") +
    (filter.DepartmentId == undefined || filter.DepartmentId == '' || filter.DepartmentId == null ? "" :
    "                                ,{ " +
    "                                    \"term\": { " +
    "                                        \"department_id\": \"" + filter.DepartmentId + "\" " +
    "                                    } " +
    "                                } ") +
    (filter.HangDoi_ID == undefined || filter.HangDoi_ID == '' || filter.HangDoi_ID == null ? "" :
    "                                ,{ " +
    "                                    \"term\": { " +
    "                                        \"id\": \"" + filter.HangDoi_ID + "\" " +
    "                                    } " +
    "                                } ") +
    (filter.Loai == undefined || filter.Loai == null || filter.Loai == '' ? "" :
    "                                ,{ " +
    "                                    \"match\": { " +
    "                                        \"loai\": \"|" + filter.Loai + "|\" " +
    "                                    } " +
    "                                } ") +
    (filter.LoginUserId == undefined || filter.LoginUserId == null || filter.LoginUserId == '' ? "" :
    "                                ,{ " +
    "                                   \"has_child\": { " +
    "                                       \"type\": \"user1\", " +
    "                                       \"query\": { " +
    "                                           \"term\": { " +
    "                                               \"owneruser_id\": \"" + filter.LoginUserId + "\" " +
    "                                           } " +
    "                                       } " +
    "                                   } " +
    "                                } ") +
    "                            ] " +
                                loaiQuery +
    "                            ,\"must_not\": [ " +
    "                                { " +
    "                                    \"match\": { " +
    "                                        \"\": \"\" " +
    "                                    } " +
    "                                } " +
    (filter.NotInLoai == undefined || filter.NotInLoai == null || filter.NotInLoai == '' ? "" :
    "                                ,{ " +
    "                                    \"match\": { " +
    "                                        \"loai\": \"|" + filter.NotInLoai + "|\" " +
    "                                    } " +
    "                                } ") +
    (filter.NotInHangDoi_ID == undefined || filter.NotInHangDoi_ID == null || filter.NotInHangDoi_ID == '' ? "" :
    "                                ,{ " +
    "                                    \"match\": { " +
    "                                        \"id\": \"" + filter.NotInHangDoi_ID + "\" " +
    "                                    } " +
    "                                } ") +
    "                            ] " +
    "                        } " +
    "                    } " +
    "                    , \"_source\": [\"id\", \"ma\", \"ten\"] " +
    "                    , \"size\": 20 " +
    "                    , \"from\": params.page * 20 " +
    "                    , \"sort\": [ " +
    "                        { " +
    "                            \"ten.keyword\": { " +
    "                                \"order\": \"asc\" " +
    "                            } " +
    "                        } " +
    "                      ] " +
    "                }); " +
    "        }, " +
    "        processResults: function (data, params) { " +
    "            params.page = params.page || 0; " +
    "            console.log(params.page); " +
    "            var results = []; " +
    "            $.each(data.hits.hits, function (index, item) { results.push({ id: item._source.id, ma: item._source.ma, ten: item._source.ten, text: item._source.ten }); }); " +
    "            return { " +
    "                results: results, " +
    "                pagination: { " +
    "                    more: (params.page * 20 + data.hits.hits.length) < data.hits.total.value " +
    "                } " +
    "            }; " +
    "        }, " +
    "        cache: true " +
    "    }, " +
    "    escapeMarkup: function (markup) { " +
    "        return markup; " +
    "    }, " +
    "    minimumInputLength: 0, " +
    "    templateResult: function (repo) { " +
    "        var markup = '';  " +
    "        if (repo.loading) return repo.text;  " +
    "        else if (repo.id == null) markup = '<table style=\"width: 100%;border-bottom: 1px solid black;\"><tr><td style=\"width:20%;padding:4px; text-align: left;\"><h3>'+ repo.ma+'</h3></td> <td style=\"text-align: left;\"><h3>'+repo.ten+'</h3></td></tr></table>';  " +
    "        else markup = '<table style=\"width: 100%;\"><tr><td style=\"color:maroon;font-weight:bold; width:20%;padding:4px; text-align: left;\">'+ repo.ma+'</td> <td style=\"text-align: left;\">'+repo.ten+'</td></tr></table>';  " +
    "        return markup;  " +
    "    }, " +
    "    templateSelection: function (repo) { " +
    "        if (repo.ma == undefined) return repo.text; " +
    "        return repo.text + \"(\" + repo.ma + \")\"; " +
    "    } " +
    "}); ";
    eval(s);
}


//Đây là hàm lấy dữ liệu theo hàm public override HangDoiCls[] PageReadingHangDoiXetNghiem(ActionSqlParamCls ActionSqlParam, HangDoiFilterCls OHangDoiFilter, ref long recordTotal) trong danh mục HangDoi
function CallInitSelect2El_HangDoiXetNghiem(Id, svUrl, place, filter) {
    var loaiQuery = '';
    if (filter.Loais != undefined && filter.Loais != null && filter.Loais != '') {
        loaiQuery +=
        "                                { " +
        "                                    \"match\": { " +
        "                                        \"loai\": \"|" + filter.Loais[0] + "|\" " +
        "                                    } " +
        "                                } "
        for (i = 0; i < filter.Loais.length; i++) {
            loaiQuery +=
            "                                ,{ " +
            "                                    \"match\": { " +
            "                                        \"loai\": \"|" + filter.Loais[i] + "|\" " +
            "                                    } " +
            "                                } "
        }
        if (loaiQuery != '')
            loaiQuery =
            "                            ,\"should\": [ " +
                                            loaiQuery +
            "                            ] ";

    }
    var s =
    "$('#' + Id).select2({ " +
    "    width: '100%', " +
    "    allowClear: true, " +
    "    placeholder: place, " +
    "    ajax: { " +
    "        id: function (e) { return e.id }, " +
    "        url: svUrl, " +
            //dataType: 'application/json',  " +
    "        delay: 250, " +
    "        type: 'POST', " +
    "        headers: { 'Content-Type': 'application/json' }, " +
            //params:{CodeField: codeField}, " +
    "        data: function (params) { " +
    "            params.page = params.page || 0; " +
    "            if (params.term == undefined) " +
    "                return JSON.stringify({ " +
    "                    \"query\": { " +
    "                        \"bool\": { " +
    "                            \"must\": [ " +
    "                                { " +
    "                                   \"term\": { " +
    "                                       \"join_field\": \"hangdoi\" " +
    "                                    } " +
    "                                }, " +
    "                                { " +
    "                                    \"has_child\": { " +
    "                                        \"type\": \"xetnghiem\", " +
    "                                        \"query\": { " +
    "                                            \"bool\": { " +
    "                                                \"must\": [ " +
    "                                                { " +
    "                                                   \"match_all\": {} " +
    "                                                } " +
    (filter.HieuLuc == undefined || filter.HieuLuc == '' || filter.HieuLuc == null ? "" :
    "                                                ,{ " +
    "                                                   \"term\": { " +
    "                                                       \"chisodvxetnghiem_hieuluc\": { " +
    "                                                           \"value\": " + filter.HieuLuc + " " +
    "                                                       } " +
    "                                                   } " +
    "                                               } ") +
    (filter.DichVu_ID == undefined || filter.DichVu_ID == '' || filter.DichVu_ID == null ? "" :
    "                                               ,{ " +
    "                                                   \"term\": { " +
    "                                                       \"dichvu_id\": \"" + filter.DichVu_ID + "\" " +
    "                                                   } " +
    "                                               } ") +
    "                                               ] " +
    "                                            } " +
    "                                        } " +
    "                                    } " +
    "                                } " +
    (filter.HieuLuc == undefined || filter.HieuLuc == '' || filter.HieuLuc == null ? "" :
    "                                ,{ " +
    "                                    \"term\": { " +
    "                                        \"hieuluc\": " + filter.HieuLuc + " " +
    "                                    } " +
    "                                } ") +
    (filter.DepartmentId == undefined || filter.DepartmentId == '' || filter.DepartmentId == null ? "" :
    "                                ,{ " +
    "                                    \"term\": { " +
    "                                        \"department_id\": \"" + filter.DepartmentId + "\" " +
    "                                    } " +
    "                                } ") +
    (filter.HangDoi_ID == undefined || filter.HangDoi_ID == '' || filter.HangDoi_ID == null ? "" :
    "                                ,{ " +
    "                                    \"term\": { " +
    "                                        \"id\": \"" + filter.HangDoi_ID + "\" " +
    "                                    } " +
    "                                } ") +
    (filter.Loai == undefined || filter.Loai == null || filter.Loai == '' ? "" :
    "                                ,{ " +
    "                                    \"match\": { " +
    "                                        \"loai\": \"|" + filter.Loai + "|\" " +
    "                                    } " +
    "                                } ") +
    (filter.LoginUserId == undefined || filter.LoginUserId == null || filter.LoginUserId == '' ? "" :
    "                                ,{ " +
    "                                   \"has_child\": { " +
    "                                       \"type\": \"user1\", " +
    "                                       \"query\": { " +
    "                                           \"term\": { " +
    "                                               \"owneruser_id\": \"" + filter.LoginUserId + "\" " +
    "                                           } " +
    "                                       } " +
    "                                   } " +
    "                                } ") +
    "                            ] " +
                                loaiQuery +
    (filter.NotInLoai == undefined || filter.NotInLoai == null || filter.NotInLoai == '' ? "" :
    "                            ,\"must_not\": [ " +
    "                                { " +
    "                                    \"match\": { " +
    "                                        \"loai\": \"|" + filter.NotInLoai + "|\" " +
    "                                    } " +
    "                                } " +
    "                            ] ") +
    "                        } " +
    "                    } " +
    "                    , \"_source\": [\"id\", \"ma\", \"ten\"] " +
    "                    , \"size\": 20 " +
    "                    , \"from\": params.page * 20 " +
    "                    , \"sort\": [ " +
    "                        { " +
    "                            \"ten.keyword\": { " +
    "                                \"order\": \"asc\" " +
    "                            } " +
    "                        } " +
    "                      ] " +
    "                }); " +
    "            else " +
    "                return JSON.stringify({ " +
    "                    \"query\": { " +
    "                        \"bool\": { " +
    "                            \"must\": [ " +
    "                                { " +
    "                                   \"term\": { " +
    "                                       \"join_field\": \"hangdoi\" " +
    "                                    } " +
    "                                }, " +
    "                                { " +
    "                                    \"bool\": { " +
    "                                        \"should\": [ " +
    "                                        { " +
    "                                            \"match_phrase_prefix\": { " +
    "                                                \"ma\": { " +
    "                                                    \"query\": params.term, " +
    "                                                    \"slop\": 3 " +
    "                                                } " +
    "                                            } " +
    "                                        }, " +
    "                                        { " +
    "                                            \"match_phrase_prefix\": { " +
    "                                                \"ten\": { " +
    "                                                    \"query\": params.term, " +
    "                                                    \"slop\": 3 " +
    "                                                } " +
    "                                            } " +
    "                                        }, " +
    "                                        { " +
    "                                            \"match_phrase_prefix\": { " +
    "                                                \"tentat\": { " +
    "                                                    \"query\": params.term, " +
    "                                                    \"slop\": 3 " +
    "                                                } " +
    "                                            } " +
    "                                        }, " +
    "                                        { " +
    "                                            \"wildcard\": { " +
    "                                                \"ma\":  \"*\" + params.term + \"*\"" +
    "                                            } " +
    "                                        }, " +
    "                                        { " +
    "                                            \"wildcard\": { " +
    "                                                \"ten\": \"*\" + params.term + \"*\" " +
    "                                            } " +
    "                                        }, " +
    "                                        { " +
    "                                            \"wildcard\": { " +
    "                                                \"tentat\": \"*\" + params.term + \"*\" " +
    "                                            } " +
    "                                        } " +
    "                                        ] " +
    "                                    } " +
    "                                }, " +
    "                                { " +
    "                                    \"has_child\": { " +
    "                                        \"type\": \"xetnghiem\", " +
    "                                        \"query\": { " +
    "                                            \"bool\": { " +
    "                                                \"must\": [ " +
    "                                                { " +
    "                                                   \"match_all\": {} " +
    "                                                } " +
    (filter.HieuLuc == undefined || filter.HieuLuc == '' || filter.HieuLuc == null ? "" :
    "                                                ,{ " +
    "                                                   \"term\": { " +
    "                                                       \"chisodvxetnghiem_hieuluc\": { " +
    "                                                           \"value\": " + filter.HieuLuc + " " +
    "                                                       } " +
    "                                                   } " +
    "                                               } ") +
    (filter.DichVu_ID == undefined || filter.DichVu_ID == '' || filter.DichVu_ID == null ? "" :
    "                                               ,{ " +
    "                                                   \"term\": { " +
    "                                                       \"dichvu_id\": \"" + filter.DichVu_ID + "\" " +
    "                                                   } " +
    "                                               } ") +
    "                                               ] " +
    "                                            } " +
    "                                        } " +
    "                                    } " +
    "                                } " +
    (filter.HieuLuc == undefined || filter.HieuLuc == '' || filter.HieuLuc == null ? "" :
    "                                ,{ " +
    "                                    \"term\": { " +
    "                                        \"hieuluc\": " + filter.HieuLuc + " " +
    "                                    } " +
    "                                } ") +
    (filter.DepartmentId == undefined || filter.DepartmentId == '' || filter.DepartmentId == null ? "" :
    "                                ,{ " +
    "                                    \"term\": { " +
    "                                        \"department_id\": \"" + filter.DepartmentId + "\" " +
    "                                    } " +
    "                                } ") +
    (filter.HangDoi_ID == undefined || filter.HangDoi_ID == '' || filter.HangDoi_ID == null ? "" :
    "                                ,{ " +
    "                                    \"term\": { " +
    "                                        \"id\": \"" + filter.HangDoi_ID + "\" " +
    "                                    } " +
    "                                } ") +
    (filter.Loai == undefined || filter.Loai == null || filter.Loai == '' ? "" :
    "                                ,{ " +
    "                                    \"match\": { " +
    "                                        \"loai\": \"|" + filter.Loai + "|\" " +
    "                                    } " +
    "                                } ") +
    (filter.LoginUserId == undefined || filter.LoginUserId == null || filter.LoginUserId == '' ? "" :
    "                                ,{ " +
    "                                   \"has_child\": { " +
    "                                       \"type\": \"user1\", " +
    "                                       \"query\": { " +
    "                                           \"term\": { " +
    "                                               \"owneruser_id\": \"" + filter.LoginUserId + "\" " +
    "                                           } " +
    "                                       } " +
    "                                   } " +
    "                                } ") +
    "                            ] " +
                                loaiQuery +
    (filter.NotInLoai == undefined || filter.NotInLoai == null || filter.NotInLoai == '' ? "" :
    "                            ,\"must_not\": [ " +
    "                                { " +
    "                                    \"match\": { " +
    "                                        \"loai\": \"|" + filter.NotInLoai + "|\" " +
    "                                    } " +
    "                                } " +
    "                            ] ") +
    "                        } " +
    "                    } " +
    "                    , \"_source\": [\"id\", \"ma\", \"ten\"] " +
    "                    , \"size\": 20 " +
    "                    , \"from\": params.page * 20 " +
    "                    , \"sort\": [ " +
    "                        { " +
    "                            \"ten.keyword\": { " +
    "                                \"order\": \"asc\" " +
    "                            } " +
    "                        } " +
    "                      ] " +
    "                }); " +
    "        }, " +
    "        processResults: function (data, params) { " +
    "            params.page = params.page || 0; " +
    "            console.log(params.page); " +
    "            var results = []; " +
    "            $.each(data.hits.hits, function (index, item) { results.push({ id: item._source.id, ma: item._source.ma, ten: item._source.ten, text: item._source.ten }); }); " +
    "            return { " +
    "                results: results, " +
    "                pagination: { " +
    "                    more: (params.page * 20 + data.hits.hits.length) < data.hits.total.value " +
    "                } " +
    "            }; " +
    "        }, " +
    "        cache: true " +
    "    }, " +
    "    escapeMarkup: function (markup) { " +
    "        return markup; " +
    "    }, " +
    "    minimumInputLength: 0, " +
    "    templateResult: function (repo) { " +
    "        var markup = '';  " +
    "        if (repo.loading) return repo.text;  " +
    "        else if (repo.id == null) markup = '<table style=\"width: 100%;border-bottom: 1px solid black;\"><tr><td style=\"width:20%;padding:4px; text-align: left;\"><h3>'+ repo.ma+'</h3></td> <td style=\"text-align: left;\"><h3>'+repo.ten+'</h3></td></tr></table>';  " +
    "        else markup = '<table style=\"width: 100%;\"><tr><td style=\"color:maroon;font-weight:bold; width:20%;padding:4px; text-align: left;\">'+ repo.ma+'</td> <td style=\"text-align: left;\">'+repo.ten+'</td></tr></table>';  " +
    "        return markup;  " +
    "    }, " +
    "    templateSelection: function (repo) { " +
    "        if (repo.ma == undefined) return repo.text; " +
    "        return repo.text + \"(\" + repo.ma + \")\"; " +
    "    } " +
    "}); ";
    eval(s);
}

//Đây là hàm lấy dữ liệu theo hàm public override DepartmentCls[] Reading(ActionSqlParamCls ActionSqlParam, DepartmentFilterCls ODepartmentFilter)
//và hàm public override DepartmentCls[] PageReading(ActionSqlParamCls ActionSqlParam, DepartmentFilterCls ODepartmentFilter, ref int TotalCount) trong StdDepartmentProcessBll
function CallInitSelect2El_Department(Id, svUrl, place, filter) {
    var loaiQuery = '';
    if (filter.Loais != undefined && filter.Loais != null && filter.Loais != '') {
        loaiQuery +=
        "                                { " +
        "                                    \"match\": { " +
        "                                        \"loai\": \"|" + filter.Loais[0] + "|\" " +
        "                                    } " +
        "                                } "
        for (i = 1; i < filter.Loais.length; i++) {
            loaiQuery +=
            "                                ,{ " +
            "                                    \"match\": { " +
            "                                        \"loai\": \"|" + filter.Loais[i] + "|\" " +
            "                                    } " +
            "                                } "
        }
        if (loaiQuery != '')
            loaiQuery =
            "                            ,\"should\": [ " +
                                            loaiQuery +
            "                            ] ";

    }

    var s =
    "$('#' + Id).select2({ " +
    "    width: '100%', " +
    "    allowClear: true, " +
    "    placeholder: place, " +
    "    ajax: { " +
    "        id: function (e) { return e.id }, " +
    "        url: svUrl, " +
            //dataType: 'application/json',  " +
    "        delay: 250, " +
    "        type: 'POST', " +
    "        headers: { 'Content-Type': 'application/json' }, " +
            //params:{CodeField: codeField}, " +
    "        data: function (params) { " +
    "            params.page = params.page || 0; " +
    "            if (params.term == undefined) " +
    "                return JSON.stringify({ " +
    "                    \"query\": { " +
    "                        \"bool\": { " +
    "                            \"must\": [ " +
    "                                { " +
    "                                   \"term\": { " +
    "                                       \"join_field\": \"department\" " +
    "                                    } " +
    "                                } " +
    (filter.OwnerId == undefined || filter.OwnerId == null || filter.OwnerId == '' ? "" :
    "                                ,{ " +
    "                                    \"term\": { " +
    "                                        \"owner_id\": \"" + filter.OwnerId + "\" " +
    "                                    } " +
    "                                } ") +
    (filter.OwnerUserId == undefined || filter.OwnerUserId == null || filter.OwnerUserId == '' ? "" :
    "                                ,{ " +
    "                                    \"term\": { " +
    "                                        \"owneruser_id\": \"" + filter.OwnerUserId + "\" " +
    "                                    } " +
    "                                } ") +
    (filter.Loai == undefined || filter.Loai == null || filter.Loai == '' ? "" :
    "                                ,{ " +
    "                                    \"match\": { " +
    "                                        \"loai\": \"|" + filter.Loai + "|\" " +
    "                                    } " +
    "                                } ") +
    (filter.LoginUserId == undefined || filter.LoginUserId == null || filter.LoginUserId == '' ? "" :
    "                                ,{ " +
    "                                   \"has_child\": { " +
    "                                       \"type\": \"user\", " +
    "                                       \"query\": { " +
    "                                           \"term\": { " +
    "                                               \"owneruser_id\": \"" + filter.LoginUserId + "\" " +
    "                                           } " +
    "                                       } " +
    "                                    } " +
    "                                } ") +
    "                            ] " +
                                loaiQuery +
    (filter.NotInKhoaPhong_ID == undefined || filter.NotInKhoaPhong_ID == null || filter.NotInKhoaPhong_ID == '' ? "" :
    "                            ,\"must_not\": [ " +
    "                                { " +
    "                                    \"term\": { " +
    "                                        \"id\": \"" + filter.NotInKhoaPhong_ID + "\" " +
    "                                    } " +
    "                                } " +
    "                            ] ") +
    "                        } " +
    "                    } " +
    "                    , \"_source\": [\"id\", \"ma\", \"ten\"] " +
    "                    , \"size\": 20 " +
    "                    , \"from\": params.page * 20 " +
    "                    , \"sort\": [ " +
    "                        { " +
    "                            \"ten.keyword\": { " +
    "                                \"order\": \"asc\" " +
    "                            } " +
    "                        } " +
    "                      ] " +
    "                }); " +
    "            else " +
    "                return JSON.stringify({ " +
    "                    \"query\": { " +
    "                        \"bool\": { " +
    "                            \"must\": [ " +
    "                                { " +
    "                                   \"term\": { " +
    "                                       \"join_field\": \"department\" " +
    "                                    } " +
    "                                } " +
    (filter.OwnerId == undefined || filter.OwnerId == null || filter.OwnerId == '' ? "" :
    "                                ,{ " +
    "                                    \"term\": { " +
    "                                        \"owner_id\": \"" + filter.OwnerId + "\" " +
    "                                    } " +
    "                                } ") +
    (filter.OwnerUserId == undefined || filter.OwnerUserId == null || filter.OwnerUserId == '' ? "" :
    "                                ,{ " +
    "                                    \"term\": { " +
    "                                        \"owneruser_id\": \"" + filter.OwnerUserId + "\" " +
    "                                    } " +
    "                                } ") +
    (filter.Loai == undefined || filter.Loai == null || filter.Loai == '' ? "" :
    "                                ,{ " +
    "                                    \"match\": { " +
    "                                        \"loai\": \"|" + filter.Loai + "|\" " +
    "                                    } " +
    "                                } ") +
    (filter.LoginUserId == undefined || filter.LoginUserId == null || filter.LoginUserId == '' ? "" :
    "                                ,{ " +
    "                                   \"has_child\": { " +
    "                                       \"type\": \"user\", " +
    "                                       \"query\": { " +
    "                                           \"term\": { " +
    "                                               \"owneruser_id\": \"" + filter.LoginUserId + "\" " +
    "                                           } " +
    "                                       } " +
    "                                    } " +
    "                                } ") +
    "                                ,{ " +
    "                                    \"bool\": { " +
    "                                        \"should\": [ " +
    "                                        { " +
    "                                            \"match_phrase_prefix\": { " +
    "                                                \"ma\": { " +
    "                                                    \"query\": params.term, " +
    "                                                    \"slop\": 3 " +
    "                                                } " +
    "                                            } " +
    "                                        }, " +
    "                                        { " +
    "                                            \"match_phrase_prefix\": { " +
    "                                                \"ten\": { " +
    "                                                    \"query\": params.term, " +
    "                                                    \"slop\": 3 " +
    "                                                } " +
    "                                            } " +
    "                                        }, " +
    "                                        { " +
    "                                            \"match_phrase_prefix\": { " +
    "                                                \"tentat\": { " +
    "                                                    \"query\": params.term, " +
    "                                                    \"slop\": 3 " +
    "                                                } " +
    "                                            } " +
    "                                        }, " +
    "                                        { " +
    "                                            \"wildcard\": { " +
    "                                                \"ma\":  \"*\" + params.term + \"*\"" +
    "                                            } " +
    "                                        }, " +
    "                                        { " +
    "                                            \"wildcard\": { " +
    "                                                \"ten\": \"*\" + params.term + \"*\" " +
    "                                            } " +
    "                                        }, " +
    "                                        { " +
    "                                            \"wildcard\": { " +
    "                                                \"tentat\": \"*\" + params.term + \"*\" " +
    "                                            } " +
    "                                        } " +
    "                                        ] " +
    "                                    } " +
    "                                } " +
    "                            ] " +
                                loaiQuery +
                                (filter.NotInKhoaPhong_ID == undefined || filter.NotInKhoaPhong_ID == null || filter.NotInKhoaPhong_ID == '' ? "" :
    "                            ,\"must_not\": [ " +
    "                                { " +
    "                                    \"term\": { " +
    "                                        \"id\": \"" + filter.NotInKhoaPhong_ID + "\" " +
    "                                    } " +
    "                                } " +
    "                            ] ") +
    "                        } " +
    "                    } " +
    "                    , \"_source\": [\"id\", \"ma\", \"ten\"] " +
    "                    , \"size\": 20 " +
    "                    , \"from\": params.page * 20 " +
    "                    , \"sort\": [ " +
    "                        { " +
    "                            \"ten.keyword\": { " +
    "                                \"order\": \"asc\" " +
    "                            } " +
    "                        } " +
    "                      ] " +
    "                }); " +
    "        }, " +
    "        processResults: function (data, params) { " +
    "            params.page = params.page || 0; " +
    "            console.log(params.page); " +
    "            var results = []; " +
    "            $.each(data.hits.hits, function (index, item) { results.push({ id: item._source.id, ma: item._source.ma, ten: item._source.ten, text: item._source.ten }); }); " +
    "            return { " +
    "                results: results, " +
    "                pagination: { " +
    "                    more: (params.page * 20 + data.hits.hits.length) < data.hits.total.value " +
    "                } " +
    "            }; " +
    "        }, " +
    "        cache: true " +
    "    }, " +
    "    escapeMarkup: function (markup) { " +
    "        return markup; " +
    "    }, " +
    "    minimumInputLength: 0, " +
    "    templateResult: function (repo) { " +
    "        var markup = '';  " +
    "        if (repo.loading) return repo.text;  " +
    "        else if (repo.id == null) markup = '<table style=\"width: 100%;border-bottom: 1px solid black;\"><tr><td style=\"width:20%;padding:4px; text-align: left;\"><h3>'+ repo.ma+'</h3></td> <td style=\"text-align: left;\"><h3>'+repo.ten+'</h3></td></tr></table>';  " +
    "        else markup = '<table style=\"width: 100%;\"><tr><td style=\"color:maroon;font-weight:bold; width:20%;padding:4px; text-align: left;\">'+ repo.ma+'</td> <td style=\"text-align: left;\">'+repo.ten+'</td></tr></table>';  " +
    "        return markup;  " +
    "    }, " +
    "    templateSelection: function (repo) { " +
    "        if (repo.ma == undefined) return repo.text; " +
    "        return repo.text + \"(\" + repo.ma + \")\"; " +
    "    } " +
    "}); ";
    eval(s);
}
//GiaDichVuChiTietProcessBll: public override GiaDichVuChiTietCls[] PageReading(ActionSqlParamCls ActionSqlParam, GiaDichVuChiTietFilterCls OGiaDichVuChiTietFilter, ref long recordTotal)
//GiaDichVuChiTietProcessBll: public override GiaDichVuChiTietCls[] PageReadingNotBuongGiuongKBDieuTri(ActionSqlParamCls ActionSqlParam, GiaDichVuChiTietFilterCls OGiaDichVuChiTietFilter, ref long recordTotal)
//HangDoiDichVuService
function CallInitSelect2El_GiaDichVu(Id, svUrl, place, filter) {
    var filterQuery =
    "                                { " +
    "                                   \"term\": { " +
    "                                       \"join_field\": \"giadichvuchitiet\" " +
    "                                    } " +
    "                                } " +
    "                                ,{ " +
    "                                   \"term\": { " +
    "                                       \"hieuLuc\": 1 " +
    "                                    } " +
    "                                } " +
    "                                ,{ " +
    "                                    \"has_parent\": { " +
    "                                        \"parent_type\": \"dichvu\", " +
    "                                        \"query\": { " +
    "                                            \"bool\": { " +
    "                                               \"must\": [ " +
    "                                                   { " +
    "                                                       \"term\": { " +
    "                                                           \"hieuLuc\": { " +
    "                                                               \"value\": 1 " +
    "                                                           } " +
    "                                                       } " +
    "                                                   } " +
    (filter.LopDichVu == undefined || filter.LopDichVu == null ? "" :
    "                                                   ,{ " +
    "                                                       \"term\": { " +
    "                                                           \"lopDichVu\": { " +
    "                                                               \"value\": " + filter.LopDichVu + " " +
    "                                                           } " +
    "                                                       } " +
    "                                                   } ") +
    (filter.HangDoiId == undefined || filter.HangDoiId == null || filter.HangDoiId == '' ? "" :
    "                                                   ,{ " +
    "                                                      \"has_child\": { " +
    "                                                          \"type\": \"hangdoidichvu\", " +
    "                                                          \"query\": { " +
    "                                                              \"term\": { " +
    "                                                                  \"hangDoi_Id\": \"" + filter.HangDoiId + "\" " +
    "                                                              } " +
    "                                                          } " +
    "                                                      } " +
    "                                                   } ") +
    "                                               ] " +
    (filter.KhacLopDichVu == undefined || filter.KhacLopDichVu == null ? "" :
    "                                               ,\"must_not\": [ " +
    "                                                   { " +
    "                                                       \"term\": { " +
    "                                                           \"lopDichVu\": { " +
    "                                                               \"value\": " + filter.KhacLopDichVu + " " +
    "                                                           } " +
    "                                                       } " +
    "                                                   } " +
    "                                               ] ") +
    "                                            } " +
    "                                        } " +
    "                                    } " +
    "                                } " +
    (filter.DoiTuongId == undefined || filter.DoiTuongId == null || filter.DoiTuongId == '' ? "" :
    "                                ,{ " +
    "                                    \"term\": { " +
    "                                        \"doiTuong_Id\": \"" + filter.DoiTuongId + "\" " +
    "                                    } " +
    "                                } ") +
    (filter.TuNgay == undefined || filter.TuNgay == null || filter.TuNgay == '' || filter.DenNgay == undefined || filter.DenNgay == null || filter.DenNgay == '' ? "" :
    "                               ,{\"bool\": { " +
    "                                   \"should\": [ " +
    "                                       { " +
    "                                           \"bool\": { " +
    "                                               \"must\": [ " +
    "                                                   { " +
    "                                                       \"term\": { " +
    "                                                           \"hieuLuc_Bgdv\": 1 " +
    "                                                       } " +
    "                                                   } " +
    "                                                   ,{ " +
    "                                                       \"range\" : { " +
    "                                                           \"tuNgay_Bgdv\" : {  " +
    "                                                               \"lte\" : \"" + filter.TuNgay + "\"  " +
    "                                                           } " +
    "                                                       } " +
    "                                                   } " +
    "                                               ] " +
    "                                           } " +
    "                                       } " +
    "                                       ,{ " +
    "                                           \"bool\": { " +
    "                                               \"must\": [ " +
    "                                                   { " +
    "                                                       \"term\": { " +
    "                                                           \"hieuLuc_Bgdv\": 0 " +
    "                                                       } " +
    "                                                   } " +
    "                                                   ,{ " +
    "                                                       \"range\" : { " +
    "                                                           \"tuNgay_Bgdv\" : {  " +
    "                                                               \"lte\" : \"" + filter.TuNgay + "\"  " +
    "                                                           } " +
    "                                                       } " +
    "                                                   } " +
    "                                                   ,{ " +
    "                                                       \"range\" : { " +
    "                                                           \"denNgay_Bgdv\" : {  " +
    "                                                               \"gte\" : \"" + filter.DenNgay + "\"  " +
    "                                                           } " +
    "                                                       } " +
    "                                                   } " +
    "                                               ] " +
    "                                           } " +
    "                                       } " +
    "                                    ] " +
    "                                }} ");
    

    var s =
    "$('#' + Id).select2({ " +
    "    width: '100%', " +
    "    allowClear: true, " +
    "    placeholder: place, " +
    "    ajax: { " +
    "        id: function (e) { return e.id }, " +
    "        url: svUrl, " +
            //dataType: 'application/json',  " +
    "        delay: 250, " +
    "        type: 'POST', " +
    "        headers: { 'Content-Type': 'application/json' }, " +
            //params:{CodeField: codeField}, " +
    "        data: function (params) { " +
    "            params.page = params.page || 0; " +
    "            if (params.term == undefined) " +
    "                return JSON.stringify({ " +
    "                    \"query\": { " +
    "                        \"bool\": { " +
    "                            \"must\": [ " +
    filterQuery +
    "                            ] " +
    "                        } " +
    "                    } " +
    "                    , \"_source\": [\"id\", \"ma\", \"ten\"] " +
    "                    , \"size\": 20 " +
    "                    , \"from\": params.page * 20 " +
    "                    , \"sort\": [ " +
    "                        { " +
    "                            \"ten.keyword\": { " +
    "                                \"order\": \"asc\" " +
    "                            } " +
    "                        } " +
    "                      ] " +
    "                }); " +
    
    "            else " +
    "                return JSON.stringify({ " +
    "                    \"query\": { " +
    "                        \"bool\": { " +
    "                            \"must\": [ " +
    filterQuery +
    "                                ,{ " +
    "                                    \"has_parent\": { " +
    "                                        \"parent_type\": \"dichvu\", " +
    "                                        \"query\": { " +
    "                                           \"bool\": { " +
    "                                               \"should\": [ " +
    "                                                   { " +
    "                                                       \"match_phrase_prefix\": { " +
    "                                                           \"ma\": { " +
    "                                                               \"query\": params.term, " +
    "                                                               \"slop\": 30 " +
    "                                                           } " +
    "                                                       } " +
    "                                                   }, " +
    "                                                   { " +
    "                                                       \"match_phrase_prefix\": { " +
    "                                                           \"ten\": { " +
    "                                                               \"query\": params.term, " +
    "                                                               \"slop\": 30 " +
    "                                                           } " +
    "                                                       } " +
    "                                                   }, " +
    "                                                   { " +
    "                                                       \"match_phrase_prefix\": { " +
    "                                                           \"tenTat\": { " +
    "                                                               \"query\": params.term, " +
    "                                                               \"slop\": 30 " +
    "                                                           } " +
    "                                                       } " +
    "                                                   }, " +
    "                                                   { " +
    "                                                       \"match_phrase_prefix\": { " +
    "                                                           \"tenGoc\": { " +
    "                                                               \"query\": params.term, " +
    "                                                               \"slop\": 30 " +
    "                                                           } " +
    "                                                       } " +
    "                                                   }, " +
    "                                                   { " +
    "                                                       \"wildcard\": { " +
    "                                                           \"ma\":  \"*\" + params.term + \"*\"" +
    "                                                       } " +
    "                                                   } " +
    "                                               ] " +
    "                                           } " +
    "                                       } " +
    "                                    } " +
    "                                } " +
    "                            ] " +
    "                        } " +
    "                    } " +
    "                    , \"_source\": [\"id\", \"ma\", \"ten\"] " +
    "                    , \"size\": 20 " +
    "                    , \"from\": params.page * 20 " +
    //"                    , \"sort\": [ " +
    //"                        { " +
    //"                            \"ten.keyword\": { " +
    //"                                \"order\": \"asc\" " +
    //"                            } " +
    //"                        } " +
    //"                      ] " +
    "                }); " +
    
    "        }, " +
    "        processResults: function (data, params) { " +
    "            params.page = params.page || 0; " +
    "            console.log(params.page); " +
    "            var results = []; " +
    "            if(params.page == 0) \r\n" +
    "               results.push({ ma: 'Mã', ten: 'Tên', text: 'Tên' , ghiChu: 'Ghi chú'}); " +
    "            $.each(data.hits.hits, function (index, item) { results.push({ id: item._source.id, ma: item._source.ma, ten: item._source.ten, text: item._source.ten , ghiChu: item._source.ghiChu, donGia: item._source.donGia, donGiaCu: item._source.donGiaCu, phuThu: item._source.phuThu}); }); " +
    "            return { " +
    "                results: results, " +
    "                pagination: { " +
    "                    more: (params.page * 20 + data.hits.hits.length) < data.hits.total.value " +
    "                } " +
    "            }; " +
    "        }, " +
    "        cache: true " +
    "    }, " +
    "    escapeMarkup: function (markup) { " +
    "        return markup; " +
    "    }, " +
    "    minimumInputLength: 0, " +
    "    templateResult: function (repo) { " +
    "        var markup = '';  " +
    "        if (repo.loading) return repo.text;  " +
    "        else if (repo.id == null) markup = '<table style=\"width: 100%;border-bottom: 1px solid black;\"><tr><td style=\"width:20%;padding:4px; text-align: left;\"><h3>'+ repo.ma+'</h3></td> <td style=\"text-align: left; width:60%;\"><h3>'+repo.ten+'</h3></td><td style=\"text-align: left; \">'+(repo.ghiChu == undefined ? '' : repo.ghiChu)+'</td></tr></table>';  " +
    "        else markup = '<table style=\"width: 100%;border-bottom: 1px solid black;\"><tr><td style=\"color:maroon;font-weight:bold; width:20%;padding:4px; text-align: left;\">'+ repo.ma+'</td> <td style=\"text-align: left; width:60%;\">'+repo.ten+'</td><td style=\"text-align: left;\">'+(repo.ghiChu == undefined ? '' : repo.ghiChu)+'</td></tr></table>';  " +
    "        return markup;  " +
    "    }, " +
    "    templateSelection: function (repo) { " +
    "        if (repo.ma == undefined) return repo.text; " +
    "        return repo.text + \"(\" + repo.ma + \")\"; " +
    "    } " +
    "}); ";
    eval(s);
}
//DichVuProcessBll: public override DichVuCls[] PageReading(ActionSqlParamCls ActionSqlParam, DichVuFilterCls ODichVuFilter, ref long recordTotal)
//DichVuProcessBll: public override DichVuCls[] GetDichVuByHangDoiId(ActionSqlParamCls ActionSqlParam, DichVuFilterCls ODichVuFilter, ref long recordTotal)
function CallInitSelect2El_DichVu(Id, svUrl, place, filter) {
    var filterQuery =
    "                                { " +
    "                                   \"term\": { " +
    "                                       \"join_field\": \"dichvu\" " +
    "                                    } " +
    "                                } " +
    "                                ,{ " +
    "                                   \"term\": { " +
    "                                       \"hieuLuc\": 1 " +
    "                                    } " +
    "                                } " +
    (filter.LopDichVu == undefined || filter.LopDichVu == null ? "" :
    "                                ,{ " +
    "                                   \"term\": { " +
    "                                       \"lopDichVu\": { " +
    "                                           \"value\": " + filter.LopDichVu + " " +
    "                                        } " +
    "                                    } " +
    "                                } ") +
    (filter.KhacLopDichVu == undefined || filter.KhacLopDichVu == null ? "" :
    "                                ,{ " +
    "                                   \"bool\": { " +
    "                                       \"must_not\": { " +
    "                                           \"term\": { " +
    "                                               \"lopDichVu\": { " +
    "                                                   \"value\": " + filter.KhacLopDichVu + " " +
    "                                               } " +
    "                                            } " +
    "                                        } " +
    "                                    } " +
    "                                } ") +
    (filter.HangDoiId == undefined || filter.HangDoiId == null || filter.HangDoiId == '' ? "" :
    "                               ,{ " +
    "                                   \"has_child\": { " +
    "                                       \"type\": \"hangdoidichvu\", " +
    "                                       \"query\": { " +
    "                                           \"term\": { " +
    "                                               \"hangDoi_Id\": \"" + filter.HangDoiId + "\" " +
    "                                           } " +
    "                                       } " +
    "                                   } " +
    "                               } ");


    var s =
    "$('#' + Id).select2({ " +
    "    width: '100%', " +
    "    allowClear: true, " +
    "    placeholder: place, " +
    "    ajax: { " +
    "        id: function (e) { return e.id }, " +
    "        url: svUrl, " +
            //dataType: 'application/json',  " +
    "        delay: 250, " +
    "        type: 'POST', " +
    "        headers: { 'Content-Type': 'application/json' }, " +
            //params:{CodeField: codeField}, " +
    "        data: function (params) { " +
    "            params.page = params.page || 0; " +
    "            if (params.term == undefined) " +
    "                return JSON.stringify({ " +
    "                    \"query\": { " +
    "                        \"bool\": { " +
    "                            \"must\": [ " +
    filterQuery +
    "                            ] " +
    "                        } " +
    "                    } " +
    "                    , \"_source\": [\"id\", \"ma\", \"ten\"] " +
    "                    , \"size\": 20 " +
    "                    , \"from\": params.page * 20 " +
    "                    , \"sort\": [ " +
    "                        { " +
    "                            \"ten.keyword\": { " +
    "                                \"order\": \"asc\" " +
    "                            } " +
    "                        } " +
    "                      ] " +
    "                }); " +

    "            else " +
    "                return JSON.stringify({ " +
    "                    \"query\": { " +
    "                        \"bool\": { " +
    "                            \"must\": [ " +
    filterQuery +
    "                                ,{ " +
    "                                   \"bool\": { " +
    "                                       \"should\": [ " +
    "                                           { " +
    "                                               \"match_phrase_prefix\": { " +
    "                                                   \"ma\": { " +
    "                                                       \"query\": params.term, " +
    "                                                       \"slop\": 30 " +
    "                                                   } " +
    "                                               } " +
    "                                           }, " +
    "                                           { " +
    "                                               \"match_phrase_prefix\": { " +
    "                                                   \"ten\": { " +
    "                                                       \"query\": params.term, " +
    "                                                       \"slop\": 30 " +
    "                                                   } " +
    "                                               } " +
    "                                           }, " +
    "                                           { " +
    "                                               \"match_phrase_prefix\": { " +
    "                                                   \"tenTat\": { " +
    "                                                       \"query\": params.term, " +
    "                                                       \"slop\": 30 " +
    "                                                   } " +
    "                                               } " +
    "                                           }, " +
    "                                           { " +
    "                                               \"match_phrase_prefix\": { " +
    "                                                   \"tenGoc\": { " +
    "                                                       \"query\": params.term, " +
    "                                                       \"slop\": 30 " +
    "                                                   } " +
    "                                               } " +
    "                                           }, " +
    "                                           { " +
    "                                               \"wildcard\": { " +
    "                                                   \"ma\":  \"*\" + params.term + \"*\"" +
    "                                               } " +
    "                                           } " +
    "                                       ] " +
    "                                   } " +
    "                                } " +
    "                            ] " +
    "                        } " +
    "                    } " +
    "                    , \"_source\": [\"id\", \"ma\", \"ten\"] " +
    "                    , \"size\": 20 " +
    "                    , \"from\": params.page * 20 " +
    //"                    , \"sort\": [ " +
    //"                        { " +
    //"                            \"ten.keyword\": { " +
    //"                                \"order\": \"asc\" " +
    //"                            } " +
    //"                        } " +
    //"                      ] " +
    "                }); " +

    "        }, " +
    "        processResults: function (data, params) { " +
    "            params.page = params.page || 0; " +
    "            console.log(params.page); " +
    "            var results = []; " +
    "            if(params.page == 0) \r\n" +
    "               results.push({ ma: 'Mã', ten: 'Tên', text: 'Tên'}); " +
    "            $.each(data.hits.hits, function (index, item) { results.push({ id: item._source.id, ma: item._source.ma, ten: item._source.ten, text: item._source.ten}); }); " +
    "            return { " +
    "                results: results, " +
    "                pagination: { " +
    "                    more: (params.page * 20 + data.hits.hits.length) < data.hits.total.value " +
    "                } " +
    "            }; " +
    "        }, " +
    "        cache: true " +
    "    }, " +
    "    escapeMarkup: function (markup) { " +
    "        return markup; " +
    "    }, " +
    "    minimumInputLength: 0, " +
    "    templateResult: function (repo) { " +
    "        var markup = '';  " +
    "        if (repo.loading) return repo.text;  " +
    "        else if (repo.id == null) markup = '<table style=\"width: 100%;border-bottom: 1px solid black;\"><tr><td style=\"width:20%;padding:4px; text-align: left;\"><h3>'+ repo.ma+'</h3></td> <td style=\"text-align: left;\"><h3>'+repo.ten+'</h3></td></tr></table>';  " +
    "        else markup = '<table style=\"width: 100%;border-bottom: 1px solid black;\"><tr><td style=\"color:maroon;font-weight:bold; width:20%;padding:4px; text-align: left;\">'+ repo.ma+'</td> <td style=\"text-align: left;\">'+repo.ten+'</td></tr></table>';  " +
    "        return markup;  " +
    "    }, " +
    "    templateSelection: function (repo) { " +
    "        if (repo.ma == undefined) return repo.text; " +
    "        return repo.text + \"(\" + repo.ma + \")\"; " +
    "    } " +
    "}); ";
    eval(s);
}

//Hàm CallInitSelect2 truy vấn dữ liệu cơ bản từ Elasticsearch khi truyền list filter với keyCode là tên bộ lọc, value là giá trị của bộ lọc
function CallInitSelect2ELFilter(Id, svUrl, place, filter) {
    var loaiQuery = '';
    if (filter.length > 0) {
        loaiQuery +=
        "                                { " +
        "                                    \"match\": { " +
        "                                        \"" + filter[0].keyCode + "\": \"" + filter[0].value + "\" " +
        "                                    } " +
        "                                } "
        for (i = 1; i < filter.length; i++) {
            loaiQuery +=
            "                                ,{ " +
            "                                    \"match\": { " +
            "                                        \"" + filter[i].keyCode + "\": \"" + filter[i].value + "\" " +
            "                                    } " +
            "                                } "
        }
    }

    var s =
    "$('#' + Id).select2({ " +
    "    width: '100%', " +
    "    allowClear: true, " +
    "    placeholder: place, " +
    "    ajax: { " +
    "        id: function (e) { return e.id }, " +
    "        url: svUrl, " +
            //dataType: 'application/json',  " +
    "        delay: 250, " +
    "        type: 'POST', " +
    "        headers: { 'Content-Type': 'application/json' }, " +
            //params:{CodeField: codeField}, " +
    "        data: function (params) { " +
    "            params.page = params.page || 0; " +
    "            if (params.term == undefined) " +
    "                return JSON.stringify({ " +
    "                    \"query\": { " +
    "                        \"bool\": { " +
    "                            \"must\": [ " +
    loaiQuery +
    "                            ] " +
    "                        } " +
    "                    } " +
    "                    , \"_source\": [\"id\", \"ma\", \"ten\"] " +
    "                    , \"size\": 20 " +
    "                    , \"from\": params.page * 20 " +
    "                    , \"sort\": [ " +
    "                        { " +
    "                            \"ten.keyword\": { " +
    "                                \"order\": \"asc\" " +
    "                            } " +
    "                        } " +
    "                      ] " +
    "                }); " +
    "            else " +
    "                return JSON.stringify({ " +
    "                    \"query\": { " +
    "                        \"bool\": { " +
    "                            \"must\": [ " +
    "                                 { " +
    "                                    \"bool\": { " +
    "                                        \"should\": [ " +
    "                                        { " +
    "                                            \"match_phrase_prefix\": { " +
    "                                                \"ma\": { " +
    "                                                    \"query\": params.term, " +
    "                                                    \"slop\": 3 " +
    "                                                } " +
    "                                            } " +
    "                                        }, " +
    "                                        { " +
    "                                            \"match_phrase_prefix\": { " +
    "                                                \"ten\": { " +
    "                                                    \"query\": params.term, " +
    "                                                    \"slop\": 3 " +
    "                                                } " +
    "                                            } " +
    "                                        }, " +
    "                                        { " +
    "                                            \"match_phrase_prefix\": { " +
    "                                                \"tentat\": { " +
    "                                                    \"query\": params.term, " +
    "                                                    \"slop\": 3 " +
    "                                                } " +
    "                                            } " +
    "                                        }, " +
    "                                        { " +
    "                                            \"wildcard\": { " +
    "                                                \"ma\":  \"*\" + params.term + \"*\"" +
    "                                            } " +
    "                                        }, " +
    "                                        { " +
    "                                            \"wildcard\": { " +
    "                                                \"ten\": \"*\" + params.term + \"*\" " +
    "                                            } " +
    "                                        }, " +
    "                                        { " +
    "                                            \"wildcard\": { " +
    "                                                \"tentat\": \"*\" + params.term + \"*\" " +
    "                                            } " +
    "                                        } " +
    "                                        ] " +
    "                                    } " +
    "                                }, " +
                                    loaiQuery +
    "                            ] " +
    "                        } " +
    "                    } " +
    "                    , \"_source\": [\"id\", \"ma\", \"ten\"] " +
    "                    , \"size\": 20 " +
    "                    , \"from\": params.page * 20 " +
    "                    , \"sort\": [ " +
    "                        { " +
    "                            \"ten.keyword\": { " +
    "                                \"order\": \"asc\" " +
    "                            } " +
    "                        } " +
    "                      ] " +
    "                }); " +
    "        }, " +
    "        processResults: function (data, params) { " +
    "            params.page = params.page || 0; " +
    "            console.log(params.page); " +
    "            var results = []; " +
    "            $.each(data.hits.hits, function (index, item) { results.push({ id: item._id, ma: item._source.ma, ten: item._source.ten, text: item._source.ten }); }); " +
    "            return { " +
    "                results: results, " +
    "                pagination: { " +
    "                    more: (params.page * 20 + data.hits.hits.length) < data.hits.total.value " +
    "                } " +
    "            }; " +
    "        }, " +
    "        cache: true " +
    "    }, " +
    "    escapeMarkup: function (markup) { " +
    "        return markup; " +
    "    }, " +
    "    minimumInputLength: 0, " +
    "    templateResult: function (repo) { " +
    "        var markup = '';  " +
    "        if (repo.loading) return repo.text;  " +
    "        else if (repo.id == null) markup = '<table style=\"width: 100%;border-bottom: 1px solid black;\"><tr><td style=\"width:20%;padding:4px; text-align: left;\"><h3>'+ repo.ma+'</h3></td> <td style=\"text-align: left;\"><h3>'+repo.ten+'</h3></td></tr></table>';  " +
    "        else markup = '<table style=\"width: 100%;\"><tr><td style=\"color:maroon;font-weight:bold; width:20%;padding:4px; text-align: left;\">'+ repo.ma+'</td> <td style=\"text-align: left;\">'+repo.ten+'</td></tr></table>';  " +
    "        return markup;  " +
    "    }, " +
    "    templateSelection: function (repo) { " +
    "        if (repo.ma == undefined) return repo.text; " +
    "        return repo.text + \"(\" + repo.ma + \")\"; " +
    "    } " +
    "}); ";
    eval(s);
}

    //Kiểm tra định dạng kiểu Time HH:mm
    function CheckTimeFormat(sTime)
    {
        var reg=/^(20|21|22|23|[0-1]?\d{1}):([0-5]?\d{1})$/;
        if(sTime.match(reg)){
             return true;
        }
        return false;
    } 

    //Kiểm tra định dạng kiểu Date dd/MM/yyyy
    function CheckDateFormat(sDate)
    { 
        var reg=/^(([0-2]?[0-9]|3[0-1])\/([0]?[1-9]|1[0-2])\/[1-2]\d{3})$/;
        if(sDate.match(reg)){
            return true;
        }
        return false;
    } 

    //Kiểm tra định dạng kiểu DateTime HH:mm dd/MM/yyyy
    function CheckDateTimeFormat(sDateTime)
    {
        var reg=/^(20|21|22|23|[0-1]?\d{1}):([0-5]?\d{1}) (([0-2]?[0-9]|3[0-1])\/([0]?[1-9]|1[0-2])\/[1-2]\d{3})$/;
        if(sDateTime.match(reg)){
            return true;
        }
        return false;
    } 


    //Validate control integer
    function ValidateIntegerControl(control, min, max)
    { 
        $(control).on('keydown keyup blur', function(e){
            if (($(this).val() > max || $(this).val() < min) && e.keyCode !== 46 && e.keyCode !== 8){
                e.preventDefault();
                $(this).val(min);
            }
            else if(e.keyCode != 8 && e.keyCode != 0 && e.keyCode != 37 && e.keyCode != 38 && e.keyCode != 39 && e.keyCode != 40 && e.keyCode != 9 && e.keyCode != 13 && (e.keyCode < 48 || e.keyCode > 57) && !(e.keyCode <= 105 && e.keyCode >= 96))
                e.preventDefault();
                //        TH: Nhập 1. không đúng định dạng, click chuột sang control mới xét lại giá trị của nó về rỗng
            else if(e.bubbles == false && e.type == 'blur' && !this.checkValidity())
                $(this).val('');
        });
    } 

    //Validate control số thực
    function ValidateNumberControl(control, min, max)
    { 
        $(control).on('keydown blur', function(e){
            var coDauCham = $(this).val().indexOf('.') !== -1;
            //          TH > max, < min thì mặc định giá trị về min
            if (($(this).val() > max || $(this).val() < min) && e.keyCode !== 46 && e.keyCode !== 8 && $(this).val() != ''){
                e.preventDefault();
                $(this).val(min);
            }
                //          Trước dấu thập phân tối thiểu 1 số, sau dấu thập phân tối đa là 2 số
            else if (coDauCham && ($(this).val().split('.')[0].length < 1 || $(this).val().split('.')[1].length > 2) && e.keyCode !== 46 && e.keyCode !== 8){
                e.preventDefault();
                $(this).val(min);
            }
                //          Không cho nhập các ký tự đặc biệt ngoại trừ dấu .
                //          !(e.keyCode <= 105 && e.keyCode >= 96) cho phép nhập các số bên phải bàn phím
            else if(e.keyCode != 8 && e.keyCode != 0 && e.keyCode != 37 && e.keyCode != 38 && e.keyCode != 39 && e.keyCode != 40 && e.keyCode != 9 && e.keyCode != 13 && e.keyCode != 190 && e.keyCode != 110 && (e.keyCode < 48 || e.keyCode > 57) && !(e.keyCode <= 105 && e.keyCode >= 96))
                e.preventDefault();
            //          TH: Nhập 1. không đúng định dạng, ấn tab hoặc enter sang control mới xét lại giá trị của nó về mặc định và focus vào control đấy
            //        else if((e.keyCode == 9 || e.keyCode == 13) && e.type == 'keydown' && !this.checkValidity()){
            //            e.preventDefault();
            //            $(this).val(min);
            //        }
            ////          TH: Nhập 1. không đúng định dạng, click chuột sang control mới xét lại giá trị của nó về rỗng
            //        else if(e.bubbles == false && e.type == 'blur' && !this.checkValidity()){
            //            e.preventDefault();
            //            $(this).val('');
            //        }
        });
    } 

    //ConvertDateTime from string to datetime
    function ConvertDateTime(sDateTime){
        dateTime = sDateTime.split(' ');

        var time = dateTime[0].split(':');
        var h = time[0];
        var m = time[1];
        var s = 0;

        var date = dateTime[1].split('/');
        var dd = date[0];
        var mm = date[1]-1;
        var yyyy = date[2];

        return new Date(yyyy,mm,dd,h,m,s);
    }

    //So sánh 2 Date dd/MM/yyyy
    function CompareDate(sDate1, sDate2)
    {
        arrDate1 = sDate1.split('/');
        arrDate2 = sDate2.split('/');
        date1 = new Date(arrDate1[2],arrDate1[1]-1,arrDate1[0]);
        date2 = new Date(arrDate2[2],arrDate2[1]-1,arrDate2[0]);
        if(date1 > date2)
        {
            return 1;
        }
        else if(date1 < date2)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }

    //So sánh 2 DateTime dd/MM/yyyy HH:mm
    function CompareDateTime(sDateTime1, sDateTime2)
    {
        arrDateTime1 = sDateTime1.split(' ');
        arrDateTime2 = sDateTime2.split(' ');
        arrDate1 = arrDateTime1[1].split('/');
        arrDate2 = arrDateTime2[1].split('/');
        arrTime1 = arrDateTime1[0].split(':');
        arrTime2 = arrDateTime2[0].split(':');
        dateTime1 = new Date(arrDate1[2], arrDate1[1]-1, arrDate1[0], arrTime1[0], arrTime1[1]);
        dateTime2 = new Date(arrDate2[2], arrDate2[1]-1, arrDate2[0], arrTime2[0], arrTime2[1]);
        if(dateTime1 > dateTime2)
        {
            return 1;
        }
        else if(dateTime1 < dateTime2)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }

    //Trả về phần cố định của biểu thức trong bộ mã.
    function GetDisplayPart(bieuThuc)
    {
        if(bieuThuc != '')
            return bieuThuc.split('{')[0];
        return null;
    }
    //Trả về chuỗi mask của biểu thức trong bộ mã.
    function GetMaskString(bieuThuc)
    {
        if(bieuThuc != '')
        {
            bieuThucArr = bieuThuc.split('{');
            //Biểu thức dạng 15/107/18/{MaBA,\d\d\d\d\d\d}
            if(bieuThucArr.length == 2)
            {
                return bieuThucArr[0] + bieuThucArr[1].split(',')[1].split('}')[0];
            }
        }
        return null;
    }
